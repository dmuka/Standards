using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Common.GenericCRUD
{
    public class GetById<T> where T : BaseEntity
    {
        public class Query(int id) : IRequest<T>
        {
            public int Id { get; } = id;
        }

        public class QueryHandler(IRepository repository, ICacheService cacheService, string cacheKey) : IRequestHandler<Query, T>
        {
            public async Task<T> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = cacheService.GetById<T>(cacheKey, request.Id);

                if (!cancellationToken.IsCancellationRequested && entity is not null) return entity;
                
                entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);

                return entity;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator(IRepository repository)
            {
                RuleLevelCascadeMode = CascadeMode.Stop;

                RuleFor(query => query.Id)
                    .GreaterThan(default(int))
                    .SetValidator(new IdValidator<T>(repository));
            }
        }
    }
}