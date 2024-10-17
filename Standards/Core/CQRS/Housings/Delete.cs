using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Housings
{
    [TransactionScope]
    public class Delete
    {
        public class Query(int id) : IRequest<int>
        {
            public int Id { get; set; } = id;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService) : IRequestHandler<Query, int>
        {
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var housing = await repository.GetByIdAsync<Housing>(request.Id, cancellationToken);

                await repository.DeleteAsync(housing, cancellationToken);

                var result = await repository.SaveChangesAsync(cancellationToken);
                
                cacheService.Remove(Cache.Housings);

                return result;
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
