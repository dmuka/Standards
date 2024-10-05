using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Core.CQRS.Housings;

[TransactionScope]
public class Create
{
    public class Query(HousingDto housingDto) : IRequest<int>
    {
        public HousingDto HousingDto { get; set; } = housingDto;
    }

    public class QueryHandler(IRepository repository) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            await repository.AddAsync(request.HousingDto, cancellationToken);

            var result = await repository.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.HousingDto)
                .NotEmpty()
                .ChildRules(filter =>
                {
                    filter.RuleFor(housing => housing.Name)
                        .NotEmpty();

                    filter.RuleFor(housing => housing.ShortName)
                        .NotEmpty();

                    filter.RuleFor(housing => housing.FloorsCount)
                        .GreaterThan(default(int));

                    filter.RuleFor(housing => housing.Address)
                        .NotEmpty();
                });
        }
    }

}