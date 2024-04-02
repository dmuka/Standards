using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Standards.Data.Repositories.Models
{
    /// <summary>
    /// This object hold the query details.
    /// </summary>
    /// <typeparam name="T">The database entity.</typeparam>
    public class BaseQueryDetails<T> where T : class
    {
        /// <summary>
        /// Gets or sets the <see cref="Expression{TDelegate}"/> list you want to pass with your EF Core query.
        /// </summary>
        public List<Expression<Func<T, bool>>> Conditions { get; set; } = new List<Expression<Func<T, bool>>>();

        /// <summary>
        /// Gets or sets the navigation entities to be eager loaded with EF Core query.
        /// </summary>
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Func{T, TResult}"/> to order by your query.
        /// </summary>
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }
    }
}
