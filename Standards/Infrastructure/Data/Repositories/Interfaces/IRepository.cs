using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Standards.Infrastructure.Data.Repositories.Models;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Standards.Infrastructure.Data.Repositories.Interfaces
{
    /// <summary>
    /// Contains all the repository methods. If you register the multiple DbContexts, it will use the last one.
    /// To use specific <see cref="DbContext"/> please use <see cref="IRepository"/>.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Begin a new database transaction.
        /// </summary>
        /// <param name="isolationLevel"><see cref="IsolationLevel"/> to be applied on this transaction. (Default to <see cref="IsolationLevel.Unspecified"/>).</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns a <see cref="IDbContextTransaction"/> instance.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets <see cref="IQueryable{T}"/> of the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>Returns <see cref="IQueryable{T}"/>.</returns>
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;

        #region Get list
        /// <summary>
        /// This method returns <see cref="IList{T}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="IList{T}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns <see cref="IList{T}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="IList{T}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(bool asNoTracking, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns <see cref="IList{T}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="includes">Navigation properties to be loaded.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="IList{T}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns <see cref="IList{T}"/> without any filter. Call only when you want to pull all the data from the source.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="includes">Navigation properties to be loaded.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="IList{T}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes a <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes a <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes a <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="includes">Navigation properties to be loaded.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an object of <see cref="QueryDetails{TEntity}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> <see cref="object"/> which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(QueryDetails<TEntity> details, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an object of <see cref="QueryDetails{TEntity}"/> as parameter and returns <see cref="List{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> <see cref="object"/> which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="List{TEntity}"/>.</returns>
        Task<IList<TEntity>> GetListAsync<TEntity>(
            QueryDetails<TEntity> details,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns <see cref="List{TProjectedType}"/> without any filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The type to which <typeparamref name="TEntity"/> will be projected.</typeparam>
        /// <param name="selectExpression">A <b>LINQ</b> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<List<TProjectedType>> GetListAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The condition on which entity list will be returned.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <see langword="null"/>.</exception>
        Task<List<TProjectedType>> GetListAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="QueryDetails{TEntity}"/> and <paramref name="selectExpression"/> as parameters and
        /// returns <see cref="List{TProjectedType}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Return <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <see langword="null"/>.</exception>
        Task<List<TProjectedType>> GetListAsync<TEntity, TProjectedType>(
            QueryDetails<TEntity> details,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns a <see cref="PaginatedList{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="details">An object of <see cref="PaginationDetails{T}"/>.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="PaginatedList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="details"/> is smaller than 1.</exception>
        Task<PaginatedList<TEntity>> GetListAsync<TEntity>(
            PaginationDetails<TEntity> details,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns a <see cref="PaginatedList{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="details">An object of <see cref="PaginationDetails{T}"/>.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="details"/> is smaller than 1.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is smaller than 1.</exception>
        Task<PaginatedList<TProjectedType>> GetListAsync<TEntity, TProjectedType>(
            PaginationDetails<TEntity> details,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TProjectedType : class;
        #endregion

        #region Get by id
        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e. tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetByIdAsync<TEntity>(object id, bool asNoTracking, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="includes">The navigation properties to be loaded.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetByIdAsync<TEntity>(
            object id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the entity
        /// if found otherwise <see langword="null"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="includes">The navigation properties to be loaded.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetByIdAsync<TEntity>(
            object id,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <paramref name="id"/> which is the primary key value of the entity and returns the specified projected entity
        /// if found otherwise null.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="id">The primary key value of the entity.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <see langword="null"/>.</exception>
        Task<TProjectedType> GetByIdAsync<TEntity, TProjectedType>(
            object id,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;
        #endregion

        #region Get entity by condition
        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity will be returned.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity will be returned.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e. tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity will be returned.</param>
        /// <param name="includes">Navigation properties to be loaded.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition on which entity will be returned.</param>
        /// <param name="includes">Navigation properties to be loaded.</param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e. tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> condition,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="QueryDetails{TEntity}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(QueryDetails<TEntity> details, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="QueryDetails{TEntity}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="asNoTracking">A <see cref="bool"/> value which determines whether the return entity will be tracked by
        /// EF Core context or not. Default value is false i.e. tracking is enabled by default.
        /// </param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(
            QueryDetails<TEntity> details,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="Expression{Func}"/> as parameter and returns <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The projected type.</typeparam>
        /// <param name="condition">The condition on which entity will be returned.</param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <see langword="null"/>.</exception>
        Task<TProjectedType> GetAsync<TEntity, TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes an <see cref="object"/> of <see cref="QueryDetails{TEntity}"/> and a <see cref="System.Linq"/> select query
        /// and returns <typeparamref name="TProjectedType"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TProjectedType">The type of the projected entity.</typeparam>
        /// <param name="details">A <see cref="QueryDetails{TEntity}"/> object which contains all the conditions and criteria
        /// on which data will be returned.
        /// </param>
        /// <param name="selectExpression">The <see cref="System.Linq"/> select  query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <typeparamref name="TProjectedType"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="selectExpression"/> is <see langword="null"/>.</exception>
        Task<TProjectedType> GetAsync<TEntity, TProjectedType>(
            QueryDetails<TEntity> details,
            Expression<Func<TEntity, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where TEntity : class;
        #endregion

        #region Exists
        /// <summary>
        /// This method checks whether the database table contains any record.
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> ExistsAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes a predicate based on which existence of the entity will be determined
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition based on which the existence will checked.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes primary key value of the entity whose existence be determined
        /// and returns <see cref="Task"/> of <see cref="bool"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="id">The primary key value of the entity whose the existence will checked.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="bool"/>.</returns>
        Task<bool> ExistsByIdAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class;
        #endregion

        #region Count
        /// <summary>
        /// This method returns all count in <see cref="int"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="int"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition based on which count will be done.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="int"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions based on which count will be done.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="int"/>.</returns>
        Task<int> GetCountAsync<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> conditions, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method returns all count in <see cref="long"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="long"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="condition">The condition based on which count will be done.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync<TEntity>(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes one or more <em>predicates</em> and returns the count in <see cref="long"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="conditions">The conditions based on which count will be done.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/> of <see cref="long"/>.</returns>
        Task<long> GetLongCountAsync<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> conditions, CancellationToken cancellationToken = default) where TEntity : class;
        #endregion

        #region Raw SQL
        /// <summary>
        /// This method takes <paramref name="sql"/> string as parameter and returns the result of the provided sql.
        /// </summary>
        /// <typeparam name="T">The <see langword="type"/> to which the result will be mapped.</typeparam>
        /// <param name="sql">The sql query string.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sql"/> is <see langword="null"/>.</exception>
        Task<IList<T>> GetFromRawSqlAsync<T>(string sql, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method takes <paramref name="sql"/> string and the value of <paramref name="parameter"/> mentioned in the sql query as parameters
        /// and returns the result of the provided sql.
        /// </summary>
        /// <typeparam name="T">The <see langword="type"/> to which the result will be mapped.</typeparam>
        /// <param name="sql">The sql query string.</param>
        /// <param name="parameter">The value of the parameter mention in the sql query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sql"/> is <see langword="null"/>.</exception>
        Task<IList<T>> GetFromRawSqlAsync<T>(string sql, object parameter, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method takes <paramref name="sql"/> string and values of the <paramref name="parameters"/> mentioned in the sql query as parameters
        /// and returns the result of the provided sql.
        /// </summary>
        /// <typeparam name="T">The <see langword="type"/> to which the result will be mapped.</typeparam>
        /// <param name="sql">The sql query string.</param>
        /// <param name="parameters">The values of the parameters mentioned in the sql query.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sql"/> is <see langword="null"/>.</exception>
        Task<IList<T>> GetFromRawSqlAsync<T>(
            string sql,
            IEnumerable<DbParameter> parameters,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// This method takes <paramref name="sql"/> string and values of the <paramref name="parameters"/> mentioned in the sql query as parameters
        /// and returns the result of the provided sql.
        /// <para>
        /// The parameters names mentioned in the query should be like p0, p1,p2 etc.
        /// </para>
        /// </summary>
        /// <typeparam name="T">The <see langword="type"/> to which the result will be mapped.</typeparam>
        /// <param name="sql">The sql query string.</param>
        /// <param name="parameters">The values of the parameters mentioned in the sql query. The values should be primitive types.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sql"/> is <see langword="null"/>.</exception>
        Task<IList<T>> GetFromRawSqlAsync<T>(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The parameters in the sql string.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// Execute raw sql command against the configured database asynchronously.
        /// </summary>
        /// <param name="sql">The sql string.</param>
        /// <param name="parameters">The parameters in the sql string.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
        #endregion

        /// <summary>
        /// Reset the DbContext state by removing all the tracked and attached entities.
        /// </summary>
        void ClearChangeTracker();

        #region Add
        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Added"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be added.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be inserted to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Added"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be added.</typeparam>
        /// <param name="entities">The <typeparamref name="TEntity"/> objects to be inserted to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// This method takes an object of <typeparamref name="TEntity"/>, adds it to the change tracker and will
        /// be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes a collection of <typeparamref name="TEntity"/> object, adds them to the change tracker and will
        /// be inserted into the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities to be inserted.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Returns <see cref="Task"/>.</returns>
        Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class;
        #endregion

        #region Update
        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Modified"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be marked as modified.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be updated to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Modified"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be marked as modified.</typeparam>
        /// <param name="entities">The entity objects to be updated to the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        #endregion

        #region Delete
        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/> object, mark the object as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be marked as deleted.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be deleted from the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// This method takes an <typeparamref name="TEntity"/>, mark the object as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entityId"/> to be marked as deleted.</typeparam>
        /// <param name="entityId">The primary key value of the entity.</param>
        void Delete<TEntity>(int entityId) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// <para>
        /// Call <see cref="SaveChangesAsync(CancellationToken)"/> to persist the changes to the database.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be marked as deleted.</typeparam>
        /// <param name="entities">The <typeparamref name="TEntity"/> objects to be deleted from the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        /// <summary>
        /// This method takes <typeparamref name="TEntity"/> object, adds to the change tracker and will
        /// be deleted from the database when <see cref="SaveChangesAsync(CancellationToken)" /> is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entity"/> to be marked as deleted.</typeparam>
        /// <param name="entity">The <typeparamref name="TEntity"/> object to be deleted from the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;

        /// <summary>
        /// This method takes <see cref="IEnumerable{TEntity}"/> objects, mark the objects as <see cref="EntityState.Deleted"/> to the <see cref="ChangeTracker"/> of the <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the <paramref name="entities"/> to be marked as deleted.</typeparam>
        /// <param name="entities">The entity objects to be deleted in the database on <see cref="SaveChangesAsync(CancellationToken)"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class;


        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the number of state entries written to the database.
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        #endregion

        #region Where

        /// <summary>
        /// This method takes <see> <cref>Expression{Func{T, bool}}</cref> </see> predicate and return result of filtering by this predicate.
        /// </summary>
        /// <param name="condition">The condition based on which filter of items will be done.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <typeparam name="T">The type of the items to be returned.</typeparam>
        /// <returns>A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the list of items that represent result of filtering.</returns>
        public Task<IList<T>> GetEntitiesByCondition<T>(Expression<Func<T, bool>> condition,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// This method takes <see> <cref>Expression{Func{T, bool}}</cref> </see> and <see> <cref>Expression{Func{T, TProjectedType}}</cref> </see> predicates and return result of filtering and projecting by these predicates.
        /// </summary>
        /// <param name="condition">The condition based on which filter of items will be done.</param>
        /// <param name="selectExpression">The condition based on which project of items will be done.</param>
        /// <param name="include">The condition based on which adding of related items will be done.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <typeparam name="T">The type of the items to be filtered.</typeparam>
        /// <typeparam name="TProjectedType">The type of the items to be returned.</typeparam>
        /// <returns>A <see cref="Task"/> that represents the asynchronous save operation. The task result contains the list of items that represent result of filtering and projecting.</returns>
        public Task<List<TProjectedType>> SelectEntitiesByCondition<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            CancellationToken cancellationToken = default)
            where T : class
            where TProjectedType : class;

        #endregion
    }
}