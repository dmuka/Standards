using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Common.GenericCRUD;

public class GetAllBaseEntity
{
    public class Query<T>(string cacheKey) : IRequest<IList<T>> where T : BaseEntity
    {
        public string CacheKey { get; } = cacheKey;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService) : IRequestHandler<Query<T>, IList<T>> where T : BaseEntity, new()
    {
        public async Task<IList<T>> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
            
            var entities = await cache.GetOrCreateAsync<T>(
                request.CacheKey,
                async (token) =>
                {
                    var result = await repository.GetListAsync<T>(token);

                    return result;
                },
                cancellationToken,
                TimeSpan.FromMinutes(absoluteExpiration),
                TimeSpan.FromMinutes(slidingExpiration));

            if (entities is null) return [];
            
            var dtos = entities
                .Select(e => new T
                {
                    Id = e.Id,
                    Name = e.Name,
                    ShortName = e.ShortName,
                    Comments = e.Comments
                }).ToList();

            return dtos;
        }
    }
}