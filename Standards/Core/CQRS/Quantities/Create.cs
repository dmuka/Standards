using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;
using Unit = Standards.Core.Models.Unit;

namespace Standards.Core.CQRS.Quantities;

[TransactionScope]
public class Create
{
    public class Query(QuantityDto quantityDto) : IRequest<int>
    {
        public QuantityDto QuantityDto { get; } = quantityDto;
    }

    public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var units =  repository.GetQueryable<Unit>()
                .Where(unit => request.QuantityDto.UnitIds
                    .Contains(unit.Id))
                .ToList();
                
            var quantity = new Quantity
            {
                Name = request.QuantityDto.Name,
                ShortName = request.QuantityDto.ShortName,
                Units = units
            };
            
            await repository.AddAsync(quantity, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(Cache.Quantities);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.QuantityDto)
                .NotEmpty()
                .ChildRules(quantity =>
                {
                    quantity.RuleFor(dto => dto.Name)
                        .NotEmpty()
                        .MaximumLength(Lengths.QuantityName);;

                    quantity.RuleFor(dto => dto.UnitIds)
                        .NotEmpty()
                        .ForEach(id => 
                            id.SetValidator(new IdValidator<Unit>(repository)));
                });
        }
    }
}