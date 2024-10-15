using System.Linq.Expressions;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Models;

public class Filter<TEntity> : IFilter<TEntity>
{
    private Expression<Func<TEntity, bool>> _expression;

    public Filter(Func<TEntity, bool> func)
    {
        _expression = h => func(h);
    }
    
    public IQueryable<TEntity> Execute(IQueryable<TEntity> query)
    {
        return query.Where(_expression);
    }
}