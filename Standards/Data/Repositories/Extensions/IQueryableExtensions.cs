﻿using Microsoft.EntityFrameworkCore;
using Standards.Data.Repositories.Models;
using System.Linq.Expressions;

namespace Standards.Data.Repositories.Extensions
{
    /// <summary>
    /// Contains <see cref="Queryable"/> extension methods for paginated list.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Convert the <see cref="IQueryable{T}"/> into paginated list.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IQueryable"/>.</typeparam>
        /// <param name="source">The type to be extended.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">Size of the pagination.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageIndex"/> is smaller than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageSize"/> is smaller than 1.</exception>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The value of pageIndex must be greater than 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The value of pageSize must be greater than 0.");
            }

            long count = await source.LongCountAsync(cancellationToken);

            int skip = (pageIndex - 1) * pageSize;

            List<T> items = await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            var paginatedList = new PaginatedList<T>(items, count, pageIndex, pageSize);

            return paginatedList;
        }

        /// <summary>
        /// Convert the <see cref="IQueryable{T}"/> into paginated list.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IQueryable"/>.</typeparam>
        /// <param name="source">The type to be extended.</param>
        /// <param name="specification">An object of <see cref="PaginationSpecification{T}"/>.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="specification"/> is smaller than 1.</exception>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            PaginationDetails<T> details,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            if (details.PageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(details), $"The value of {nameof(details.PageIndex)} must be greater than 0.");
            }

            if (details.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(details), $"The value of {nameof(details.PageSize)} must be greater than 0.");
            }

            IQueryable<T> countSource = source;

            // modify the IQueryable using the details expression criteria
            if (details.Conditions != null && details.Conditions.Count != 0)
            {
                foreach (Expression<Func<T, bool>> condition in details.Conditions)
                {
                    countSource = countSource.Where(condition);
                }
            }

            long count = await countSource.LongCountAsync(cancellationToken);

            source = source.GetSpecifiedQuery(details);

            List<T> items = await source.ToListAsync(cancellationToken);

            PaginatedList<T> paginatedList = new PaginatedList<T>(items, count, details.PageIndex, details.PageSize);

            return paginatedList;
        }
        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, QueryDetails<T> details) where T : class
        {
            var query = GetSpecifiedQuery(inputQuery, (BaseQueryDetails<T>)details);

            // Apply paging if enabled
            if (details.Skip != null)
            {
                if (details.Skip < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(details), $"The value of {nameof(details.Skip)} in {nameof(details)} can not be negative.");
                }

                query = query.Skip((int)details.Skip);
            }

            if (details.Take != null)
            {
                if (details.Take < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(details), $"The value of {nameof(details.Take)} in {nameof(details)} can not be negative.");
                }

                query = query.Take((int)details.Take);
            }

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, PaginationDetails<T> details) where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            if (details.PageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(details), "The value of specification.PageIndex must be greater than 0.");
            }

            if (details.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(details), "The value of specification.PageSize must be greater than 0.");
            }

            var query = GetSpecifiedQuery(inputQuery, (BaseQueryDetails<T>)details);

            // Apply paging if enabled
            int skip = (details.PageIndex - 1) * details.PageSize;

            query = query.Skip(skip).Take(details.PageSize);

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, BaseQueryDetails<T> details)
            where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            IQueryable<T> query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (details.Conditions != null && details.Conditions.Count != 0)
            {
                foreach (Expression<Func<T, bool>> specificationCondition in details.Conditions)
                {
                    query = query.Where(specificationCondition);
                }
            }

            // Includes all expression-based includes
            if (details.Includes != null)
            {
                query = details.Includes(query);
            }

            // Apply ordering if expressions are set
            if (details.OrderBy != null)
            {
                query = details.OrderBy(query);
            }
            //else if (!string.IsNullOrWhiteSpace(details.OrderByDynamic.ColumnName) && !string.IsNullOrWhiteSpace(details.OrderByDynamic.SortDirection))
            //{
            //    query = query.OrderBy(details.OrderByDynamic.ColumnName + " " + details.OrderByDynamic.SortDirection);
            //}

            return query;
        }
    }
}