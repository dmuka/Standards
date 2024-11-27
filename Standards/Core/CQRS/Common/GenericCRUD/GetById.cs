using FluentValidation;
using MediatR;
using Standards.Core.Models.Interfaces;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Common.GenericCRUD;

public class GetById
{
    public class Query<T>(int id) : IRequest<T> where T : BaseEntity, ICacheable
    {
        public int Id { get; } = id;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Query<T>, T> where T : BaseEntity, ICacheable
    {
        public async Task<T> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var entity = cacheService.GetById<T>(T.GetCacheKey(), request.Id);

            if (!cancellationToken.IsCancellationRequested && entity is not null) return entity;
            
            entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);

            return entity;
        }
    }

    public class QueryValidator<T> : AbstractValidator<Query<T>> where T : BaseEntity, ICacheable
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