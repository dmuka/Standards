using Microsoft.EntityFrameworkCore;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.Infrastructure.QueryableWrapper.Implementation
{
    public class QueryableWrapper<T> : IQueryableWrapper<T> where T : class
    {
        public async Task<IList<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            return await query.ToListAsync(cancellationToken);    
        }
    }
}
