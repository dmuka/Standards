using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Domain;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Common.GenericCRUD;

public class GetById
{
    public class Query<T>(int id) : IRequest<T> where T : BaseEntity, ICacheable
    {
        public int Id { get; } = id;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService,
        ILogger<GetById> logger) : IRequestHandler<Query<T>, T?> where T : BaseEntity, ICacheable
    {
        public async Task<T?> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return null;
            
            var entity = cacheService.GetById<T>(T.GetCacheKey(), request.Id);
            
            if (entity is not null) return entity;
            
            entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);

            if (entity is null) return null;
            cacheService.Create(T.GetCacheKey(), entity);
                
            return entity;
        }
    }

    public class QueryValidator<T> : AbstractValidator<Query<T>> where T : BaseEntity, ICacheable
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Id)
                .GreaterThan(0)
                .SetValidator(new IdValidator<T>(repository));
        }
    }
}