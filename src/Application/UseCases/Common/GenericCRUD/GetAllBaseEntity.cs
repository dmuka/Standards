using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Core.Results;
using Domain.Constants;
using Domain.Models.Interfaces;
using MediatR;
using Entity = Domain.Models.Entity;

namespace Application.UseCases.Common.GenericCRUD;

public class GetAllBaseEntity
{
    public class Query<T> : IRequest<Result<List<T>>> where T : Entity, ICacheable
    {
    }

    public class QueryHandler<T>(
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query<T>, Result<List<T>>> where T : Entity, ICacheable, new()
    {
        public async Task<Result<List<T>>> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
            
            var entities = await cache.GetOrCreateAsync<T>(
                T.GetCacheKey(),
                [],
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            return entities is null ? [] : entities.ToList();
        }
    }
}