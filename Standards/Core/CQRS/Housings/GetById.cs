using FluentValidation;
using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Housings
{
    public class GetById
    {
        public class Query(int id) : IRequest<HousingDto>
        {
            public int Id { get; set; } = id;
        }

        public class QueryHandler(IRepository repository) : IRequestHandler<Query, HousingDto>
        {
            public async Task<HousingDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = await repository.GetByIdAsync<HousingDto>(request.Id, cancellationToken);

                return housing;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.Id)
                    .GreaterThan(default(int))
                    .IdValidator<Query, HousingDto>(repository);
            }
        }
    }
}