using MediatR;
using Standards.Core.CQRS.Common.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.Core.CQRS.Common.GenericCRUD;

public class GetAllBaseEntity<T> where T : BaseEntity, new()
{
    public class Query : IRequest<IList<T>>
    {
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cache, 
        IConfigService configService,
        string cacheKey) : IRequestHandler<Query, IList<T>>
    {
        public async Task<IList<T>> Handle(Query request, CancellationToken cancellationToken)
        {
            var absoluteExpiration = configService.GetValue<int>(Cache.AbsoluteExpirationConfigurationSectionKey);
            var slidingExpiration = configService.GetValue<int>(Cache.SlidingExpirationConfigurationSectionKey);
            
            var entities = await cache.GetOrCreateAsync<T>(
                cacheKey,
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