using System.Linq.Expressions;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Models;

public class Filter<TEntity>(Expression<Func<TEntity, bool>> func) : BaseFilter, IFilter<TEntity>
{
    public IQueryable<TEntity> Execute(IQueryable<TEntity> query)
    {
        return query.Where(func);
    }
}