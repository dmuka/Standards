using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Housings
{
    public class GetById
    {
        public class Query(int id) : IRequest<Housing>
        {
            public int Id { get; } = id;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, Housing>
        {
            public async Task<Housing> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = cacheService.GetById<Housing>(Cache.Housings, request.Id);

                if (!cancellationToken.IsCancellationRequested && housing is not null) return housing;
                
                housing = await repository.GetByIdAsync<Housing>(request.Id, cancellationToken);

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
                    .SetValidator(new IdValidator<Housing>(repository));
            }
        }
    }
}