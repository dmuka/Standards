using FluentValidation;
using MediatR;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Housings
{
    public class GetById
    {
        public class Query : IRequest<HousingDto>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, HousingDto>
        {
            private readonly IRepository _repository;

            public QueryHandler(IRepository repository)
            {
                _repository = repository;
            }

            public async Task<HousingDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = await _repository.GetByIdAsync<HousingDto>(request.Id, cancellationToken);

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