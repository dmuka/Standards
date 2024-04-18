using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Standards.Infrastructure.Data.Repositories.Extensions;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Data.Repositories.Models;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;

namespace Standards.Infrastructure.Data.Repositories.Implementations
{
    internal sealed class Repository<TDbContext> : IRepository where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

            return dbContextTransaction;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        #region GetListAsync
        public Task<List<T>> GetListAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            return GetListAsync<T>(false, cancellationToken);
        }

        public Task<List<T>> GetListAsync<T>(bool asNoTracking, CancellationToken cancellationToken = default) where T : class
        {
            Func<IQueryable<T>, IIncludableQueryable<T, object>> nullValue = null;

            return GetListAsync(nullValue, asNoTracking, cancellationToken);
        }

        public Task<List<T>> GetListAsync<T>(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default) where T : class
        {
            return GetListAsync(includes, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return items;
        }

        public Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : class
        {
            return GetListAsync(condition, false, cancellationToken);
        }

        public Task<List<T>> GetListAsync<T>(
            Expression<Func<T, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where T : class
        {
            return GetListAsync(condition, null, asNoTracking, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return items;
        }

        public Task<List<T>> GetListAsync<T>(QueryDetails<T> details, CancellationToken cancellationToken = default) where T : class
        {
            return GetListAsync(details, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            QueryDetails<T> specification,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            var entities = await _dbContext.Set<T>()
                .Select(selectExpression).ToListAsync(cancellationToken).ConfigureAwait(false);

            return entities;
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            var projectedEntites = await query.Select(selectExpression)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return projectedEntites;
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            QueryDetails<T> details,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (details != null)
            {
                query = query.GetSpecifiedQuery(details);
            }

            return await query.Select(selectExpression)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaginatedList<T>> GetListAsync<T>(
            PaginationDetails<T> details,
            CancellationToken cancellationToken = default) where T : class
        {
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            var paginatedList = await _dbContext.Set<T>().ToPaginatedListAsync(details, cancellationToken);

            return paginatedList;
        }

        public async Task<PaginatedList<TProjectedType>> GetListAsync<T, TProjectedType>(
            PaginationDetails<T> details,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
            where TProjectedType : class
        {
            if (details == null)
            {
                throw new ArgumentNullException(nameof(details));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            var query = _dbContext.Set<T>().GetSpecifiedQuery((BaseQueryDetails<T>)details);

            var paginatedList = await query.Select(selectExpression)
                .ToPaginatedListAsync(details.PageIndex, details.PageSize, cancellationToken);

            return paginatedList;
        }
        #endregion

        #region GetByIdAsync
        public Task<T> GetByIdAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetByIdAsync<T>(id, false, cancellationToken);
        }

        public Task<T> GetByIdAsync<T>(object id, bool asNoTracking, CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetByIdAsync<T>(id, null, asNoTracking, cancellationToken);
        }

        public Task<T> GetByIdAsync<T>(
            object id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return GetByIdAsync(id, includes, false, cancellationToken);
        }

        public async Task<T> GetByIdAsync<T>(
            object id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entityType = _dbContext.Model.FindEntityType(typeof(T));

            var primaryKeyProperties = entityType.FindPrimaryKey().Properties;
            var primaryKeyName = primaryKeyProperties.Select(p => p.Name).FirstOrDefault();
            var primaryKeyType = primaryKeyProperties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primaryKeyValue = null;

            try
            {
                primaryKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            var parameter = Expression.Parameter(typeof(T), "entity");
            var member = Expression.Property(parameter, primaryKeyName);
            var constant = Expression.Constant(primaryKeyValue, primaryKeyType);
            var body = Expression.Equal(member, constant);
            var expressionTree = Expression.Lambda<Func<T, bool>>(body, parameter);

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            var entity = await query.FirstOrDefaultAsync(expressionTree, cancellationToken).ConfigureAwait(true);

            return entity;
        }

        public async Task<TProjectedType> GetByIdAsync<T, TProjectedType>(
            object id,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            var entityType = _dbContext.Model.FindEntityType(typeof(T));

            var primaryKeyProperties = entityType.FindPrimaryKey().Properties;
            var primaryKeyName = primaryKeyProperties.Select(p => p.Name).FirstOrDefault();
            var primaryKeyType = primaryKeyProperties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primaryKeyValue = null;

            try
            {
                primaryKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            var parameter = Expression.Parameter(typeof(T), "entity");
            var member = Expression.Property(parameter, primaryKeyName);
            var constant = Expression.Constant(primaryKeyValue, primaryKeyType);
            var body = Expression.Equal(member, constant);
            var expressionTree = Expression.Lambda<Func<T, bool>>(body, parameter);

            IQueryable<T> query = _dbContext.Set<T>();

            return await query.Where(expressionTree)
                              .Select(selectExpression)
                              .FirstOrDefaultAsync(cancellationToken)
                              .ConfigureAwait(false);
        }
        #endregion

        #region GetAsyncByCondition
        public Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            CancellationToken cancellationToken = default) where T : class
        {
            return GetAsync(condition, null, false, cancellationToken);
        }

        public Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
           where T : class
        {
            return GetAsync(condition, null, asNoTracking, cancellationToken);
        }

        public Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default)
           where T : class
        {
            return GetAsync(condition, includes, false, cancellationToken);
        }

        public async Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TProjectedType> GetAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region GetAsyncByQueryDetails
        public Task<T> GetAsync<T>(QueryDetails<T> details, CancellationToken cancellationToken = default) where T : class
        {
            return GetAsync(details, false, cancellationToken);
        }

        public async Task<T> GetAsync<T>(QueryDetails<T> details, bool asNoTracking, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (details != null)
            {
                query = query.GetSpecifiedQuery(details);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TProjectedType> GetAsync<T, TProjectedType>(
            QueryDetails<T> details,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default) where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (details != null)
            {
                query = query.GetSpecifiedQuery(details);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Exists
        public Task<bool> ExistsAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            return ExistsAsync<T>(null, cancellationToken);
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition == null)
            {
                return await query.AnyAsync(cancellationToken);
            }

            var isExists = await query.AnyAsync(condition, cancellationToken).ConfigureAwait(false);

            return isExists;
        }

        public async Task<bool> ExistsByIdAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var entityType = _dbContext.Model.FindEntityType(typeof(T));

            var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primaryKeyValue = null;

            try
            {
                primaryKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primaryKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = _dbContext.Set<T>();

            bool isExistent = await query.AnyAsync(expressionTree, cancellationToken).ConfigureAwait(false);

            return isExistent;
        }
        #endregion

        #region GetCount
        public async Task<int> GetCountAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            int count = await _dbContext.Set<T>().CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<int> GetCountAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.CountAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> GetCountAsync<T>(IEnumerable<Expression<Func<T, bool>>> conditions, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (conditions != null)
            {
                foreach (Expression<Func<T, bool>> expression in conditions)
                {
                    query = query.Where(expression);
                }
            }

            return await query.CountAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<long> GetLongCountAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            long count = await _dbContext.Set<T>().LongCountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<long> GetLongCountAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.LongCountAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<long> GetLongCountAsync<T>(IEnumerable<Expression<Func<T, bool>>> conditions, CancellationToken cancellationToken = default) where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (conditions != null)
            {
                foreach (Expression<Func<T, bool>> expression in conditions)
                {
                    query = query.Where(expression);
                }
            }

            return await query.LongCountAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Raw SQL - returns items
        public async Task<List<T>> GetFromRawSqlAsync<T>(string sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            var parameters = new List<object>();

            var items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);

            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(
            string sql,
            object parameter,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            var parameters = new List<object>() { parameter };

            var items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);

            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(
            string sql,
            IEnumerable<DbParameter> parameters,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            List<T> items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(
            string sql,
            IEnumerable<object> parameters,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            var items = await _dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);

            return items;
        }
        #endregion

        #region Insert
        public async Task<object[]> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entityEntry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var primaryKeyValue = entityEntry.Metadata.FindPrimaryKey().Properties.
                Select(p => entityEntry.Property(p.Name).CurrentValue).ToArray();

            return primaryKeyValue;
        }

        public async Task InsertAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Raw SQL - returns the number of affected rows
        public Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }
        #endregion

        #region Add
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Update
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var trackedEntity = _dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(TEntity).Name} is not part of EF Core DbContext model");
                }

                var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    var primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    var primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _dbContext.Set<TEntity>().Update(entity);
            }
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public async Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var trackedEntity = _dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                var entityType = _dbContext.Model.FindEntityType(typeof(TEntity)) ?? throw new InvalidOperationException($"{typeof(TEntity).Name} is not part of EF Core DbContext model");
                var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    var primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    var primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _dbContext.Set<TEntity>().Update(entity);
            }

            var count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return count;
        }

        public async Task<int> UpdateAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<T>().UpdateRange(entities);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }
        #endregion

        #region Delete
        public async Task<int> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<T>().Remove(entity);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return count;
        }

        public async Task<int> DeleteAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<T>().RemoveRange(entities);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Delete<TEntity>(int entityId) where TEntity : class
        {
            var entity = _dbContext.Set<TEntity>().FindAsync(entityId).GetAwaiter().GetResult();
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException(nameof(entityId), "The entity id value of the entity to be deleted is not valid.");
            }

            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChangesAsync();
        }

        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().RemoveRange(entities);
        }
        #endregion

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public void ClearChangeTracker()
        {
            _dbContext.ChangeTracker.Clear();
        }

    }
}