using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Validators;
using System.Data;

namespace Standards.Core.CQRS.Housings
{
    [TransactionScope]
    public class Edit
    {
        public class Query : IRequest<int>
        {
            public Query(HousingDto housingDto)
            {
                HousingDto = housingDto;
            }

            public HousingDto HousingDto{ get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, int>
        {
            private readonly IRepository _repository;

            public QueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                _repository.Update(request.HousingDto);

                var result = await _repository.SaveChangesAsync(cancellationToken);

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
                        housing.RuleFor(_ => _.Id)
                            .GreaterThan(default(int))
                            .IdValidator(repository);

                        housing.RuleFor(_ => _.Name)
                            .NotEmpty();

                        housing.RuleFor(_ => _.ShortName)
                            .NotEmpty();

                        housing.RuleFor(_ => _.FloorsCount)
                            .GreaterThan(default(int));

                        housing.RuleFor(_ => _.Address)
                            .NotEmpty();
                    });
            }
        }
    }
}
