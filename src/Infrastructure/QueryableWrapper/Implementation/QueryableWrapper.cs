using Infrastructure.QueryableWrapper.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.QueryableWrapper.Implementation;

public class QueryableWrapper<T> : IQueryableWrapper<T> where T : class
{
    public async Task<IList<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
    {
        return await query.ToListAsync(cancellationToken);    
    }
}