using Microsoft.Extensions.Caching.Memory;
using Standards.Infrastructure.Services.Cache.Interfaces;

namespace Standards.Infrastructure.Services.Cache.IImplimentations;

public class CacheService(IMemoryCache cache) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(
        string cacheKey, 
        Func<CancellationToken, Task<T>> retrieveData,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        if (!cache.TryGetValue(cacheKey, out T cachedData))
        {
            cachedData = await retrieveData(cancellationToken);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(60),
                AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromMinutes(5)
            };
            
            cache.Set(cacheKey, cachedData, cacheEntryOptions);
        }

        return cachedData;
    }
}