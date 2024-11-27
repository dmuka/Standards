using Standards.Core;

namespace Standards.Infrastructure.Services.Interfaces;

public interface ICacheService
{
    Task<IList<T>> GetOrCreateAsync<T>(
        string cacheKey, 
        Func<CancellationToken, Task<IList<T>>> retrieveData,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    public void Create<T>(
        string cacheKey,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    public T? GetById<T>(string cacheKey, int id) where T : BaseEntity;

    public void Remove(string cacheKey);
}