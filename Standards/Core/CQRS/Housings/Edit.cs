using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Housings
{
    [TransactionScope]
    public class Edit
    {
        public class Query(HousingDto housingDto) : IRequest<int>
        {
            public HousingDto HousingDto{ get; set; } = housingDto;
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                repository.Update(request.HousingDto);

                var result = await repository.SaveChangesAsync(cancellationToken);

                return result;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.HousingDto)
                    .NotEmpty()
                    .ChildRules(housing =>
                    {
                        housing.RuleFor(housingDto => housingDto.Id)
                            .GreaterThan(default(int))
                            .IdValidator(repository);

                        housing.RuleFor(housingDto => housingDto.Name)
                            .NotEmpty();

                        housing.RuleFor(housingDto => housingDto.ShortName)
                            .NotEmpty();

                        housing.RuleFor(housingDto => housingDto.FloorsCount)
                            .GreaterThan(default(int));

                        housing.RuleFor(housingDto => housingDto.Address)
                            .NotEmpty();
                    });
            }
        }
    }
}
