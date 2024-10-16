namespace Standards.Infrastructure.Services.Cache.Interfaces;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(
        string cacheKey, 
        Func<CancellationToken, Task<T>> retrieveData,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);
}