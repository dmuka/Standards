namespace Standards.Infrastructure.Services.Interfaces;

public interface ICacheService
{
    Task<IList<T>> GetOrCreateAsync<T>(
        string cacheKey, 
        Func<CancellationToken, Task<IList<T>>> retrieveData,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);
}