using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;
using Standards.Core;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Validators;

namespace Application.CQRS.Common.GenericCRUD;

[TransactionScope]
public class Delete 
{
    public class Query<T>(int id) : IRequest<int> where T : BaseEntity
    {
        public int Id { get; } = id;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Query<T>, int> where T : BaseEntity, ICacheable
    {
        public async Task<int> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);

            await repository.DeleteAsync(entity, cancellationToken);

            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(T.GetCacheKey());

            return result;
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