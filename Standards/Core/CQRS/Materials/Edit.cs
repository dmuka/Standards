using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Services;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;
using Unit = Standards.Core.Models.Unit;

namespace Standards.Core.CQRS.Materials;

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
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Material>(repository));
                    
                    dto.RuleFor(material => material.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);
                    
                    dto.RuleFor(material => material.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.EntityName);

                    dto.RuleFor(material => material.UnitId)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<Unit>(repository));
                });
        }
    }
}