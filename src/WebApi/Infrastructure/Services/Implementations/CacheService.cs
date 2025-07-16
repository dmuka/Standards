using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace WebApi.Infrastructure.Services.Implementations;

public class CacheService(IMemoryCache cache, IRepository repository) : ICacheService
{
    public async Task<IList<T>> GetOrCreateAsync<T>(
        string cacheKey,
        Expression<Func<T, object?>>[] includes,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null) where T : class
    {
        if (cache.TryGetValue(cacheKey, out IList<T>? cachedData) && cachedData is not null) return cachedData;
        
        var query = repository.GetQueryable<T>();
            
        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        cachedData = await repository.QueryableToListAsync(query, cancellationToken);
            
        var cacheEntryOptions = GetOptions(absoluteExpiration, slidingExpiration);
            
        cache.Set(cacheKey, cachedData, cacheEntryOptions);

        return cachedData ?? [];
    }
    
    public T? GetById<T>(string cacheKey, int id) where T : BaseEntity
    {
        var entities = cache.Get<IList<T>>(cacheKey);
        
        var entity = entities?.FirstOrDefault(entity => entity.Id == id);

        return entity;
    }

    public void Create<T>(
        string cacheKey,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        if (cache.TryGetValue(cacheKey, out _)) return;
        
        var cacheEntryOptions = GetOptions(absoluteExpiration, slidingExpiration);
            
        cache.Set(cacheKey, value, cacheEntryOptions);
    }

    public void Remove(string cacheKey)
    {
        if (cache.TryGetValue(cacheKey, out _)) cache.Remove(cacheKey);
    }

    private MemoryCacheEntryOptions GetOptions(
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(2),
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromMinutes(5)
        };

        return cacheEntryOptions;
    }
}