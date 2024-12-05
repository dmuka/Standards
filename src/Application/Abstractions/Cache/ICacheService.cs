using System.Linq.Expressions;
using Domain;

namespace Application.Abstractions.Cache;

public interface ICacheService
{
    Task<IList<T>> GetOrCreateAsync<T>(
        string cacheKey,
        Expression<Func<T, object>>[] includes,
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null) where T : class;

    public void Create<T>(
        string cacheKey,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    public T? GetById<T>(string cacheKey, int id) where T : BaseEntity;

    public void Remove(string cacheKey);
}