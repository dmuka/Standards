using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Services;
using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using Unit = Domain.Models.Unit;

namespace Application.CQRS.Materials;

[TransactionScope]
public class Create
{
    public class Query(MaterialDto dtoDto) : IRequest<int>
    {
        public MaterialDto MaterialDto { get; } = dtoDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var unit =  await repository.GetByIdAsync<Unit>(request.MaterialDto.UnitId, cancellationToken);
                
            var dto = new Material
            {
                Id = request.MaterialDto.Id,
                Name = request.MaterialDto.Name,
                ShortName = request.MaterialDto.ShortName,
                Unit = unit
            };

            if (request.MaterialDto.Comments is not null) dto.Comments = request.MaterialDto.Comments;
            
            await repository.AddAsync(dto, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Materials);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.MaterialDto)
                .NotEmpty()
                .ChildRules(dto =>
                {
                    dto.RuleFor(material => material.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);
                    
                    dto.RuleFor(material => material.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    dto.RuleFor(material => material.UnitId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Unit>(repository));
                });
        }
    }
}