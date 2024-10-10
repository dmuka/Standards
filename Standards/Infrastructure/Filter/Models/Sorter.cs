using System.Linq.Expressions;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Models;

public class Sorter<TEntity, TKey>(Expression<Func<TEntity, TKey>> action) : BaseFilter, IFilter<TEntity>
{
    public IQueryable<TEntity> Execute(IQueryable<TEntity> query)
    {
        query = query.OrderBy(action);
            
        return query;
    }
}