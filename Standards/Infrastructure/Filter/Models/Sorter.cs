using System.Linq.Expressions;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Models;

public class Sorter<TEntity, TKey> : IFilter<TEntity>
{
    private Expression<Func<TEntity, TKey>> _keySelector;

    public Sorter(Func<TEntity, TKey> func)
    {
        _keySelector = h => func(h);
    }

    public IQueryable<TEntity> Execute(IQueryable<TEntity> query)
    {
        query = query.OrderBy(_keySelector);
            
        return query;
    }
}