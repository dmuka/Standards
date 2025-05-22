using Application.Abstractions.Cache;
using Application.UseCases.Common.Attributes;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Services;
using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;
using MediatR;
using Unit = Domain.Models.Unit;

namespace Application.UseCases.Materials;

[TransactionScope]
public class Edit
{
    public class Query(MaterialDto materialDto) : IRequest<int>
    {
        public MaterialDto MaterialDto { get; } = materialDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var unit =  await repository.GetByIdAsync<Unit>(request.MaterialDto.UnitId, cancellationToken);
                
            var material = new Material
            {
                Id = request.MaterialDto.Id,
                Name = request.MaterialDto.Name,
                ShortName = request.MaterialDto.ShortName,
                Unit = unit,
                Comments = request.MaterialDto.Comments
            };
                
            repository.Update(material);

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
                    dto.RuleFor(material => material.Id)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Material>(repository));
                    
                    dto.RuleFor(material => material.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);
                    
                    dto.RuleFor(material => material.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);

                    dto.RuleFor(material => material.UnitId)
                        .GreaterThan(0)
                        .SetValidator(new IdValidator<Unit>(repository));
                });
        }
    }
}