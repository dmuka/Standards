﻿namespace Infrastructure.Data.Repositories.Models
{
    /// <summary>
    /// This object holds the pagination query specifications.
    /// </summary>
    /// <typeparam name="T">The database entity i.e. an <see cref="DbSet{TEntity}"/> object.</typeparam>
    public class PaginationDetails<T> : BaseQueryDetails<T> where T : class
    {
        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}
