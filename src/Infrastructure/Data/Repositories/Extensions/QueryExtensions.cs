using System.Data.Common;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.Extensions;

internal static class QueryExtensions
{
    public static async Task<IList<T>> GetFromQueryAsync<T>(
        this DbContext dbContext,
        string sql,
        IEnumerable<object> parameters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

        await using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        
        var index = 0;

        foreach (var item in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = "@p" + index;
            dbParameter.Value = item;
            command.Parameters.Add(dbParameter);
            index++;
        }

        try
        {
            await dbContext.Database.OpenConnectionAsync(cancellationToken);

            await using var result = await command.ExecuteReaderAsync(cancellationToken);

            var list = new List<T>();

            while (await result.ReadAsync(cancellationToken))
            {
                T obj;
                    
                if (!(typeof(T).IsPrimitive || typeof(T) == typeof(string)))
                {
                    obj = Activator.CreateInstance<T>();
                    
                    var properties = obj is null ? [] : obj.GetType().GetProperties();
                    
                    foreach (var prop in properties)
                    {
                        var propertyName = prop.Name;
                        var isColumnExistent = result.ColumnExists(propertyName);

                        if (isColumnExistent)
                        {
                            var columnValue = result[propertyName];

                            if (!Equals(columnValue, DBNull.Value))
                            {
                                prop.SetValue(obj, columnValue, null);
                            }
                        }
                    }

                    list.Add(obj);
                }
                else
                {
                    obj = (T)Convert.ChangeType(result[0], typeof(T), CultureInfo.InvariantCulture);
                    list.Add(obj);
                }
            }

            return list;
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    public static async Task<IList<T>> GetFromQueryAsync<T>(
        this DbContext dbContext,
        string sql,
        IEnumerable<DbParameter> parameters,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        await using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = sql;
        
        foreach (var dbParameter in parameters)
        {
            command.Parameters.Add(dbParameter);
        }

        try
        {
            await dbContext.Database.OpenConnectionAsync(cancellationToken);

            await using var result = await command.ExecuteReaderAsync(cancellationToken);

            var list = new List<T>();

            while (await result.ReadAsync(cancellationToken))
            {
                T obj;
                if (!(typeof(T).IsPrimitive || typeof(T).Equals(typeof(string))))
                {
                    obj = Activator.CreateInstance<T>();

                    if (obj is null) continue;
                    
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        var propertyName = prop.Name;
                        var columnExists = result.ColumnExists(propertyName);

                        if (!columnExists) continue;
                        
                        var columnValue = result[propertyName];

                        if (!Equals(columnValue, DBNull.Value))
                        {
                            prop.SetValue(obj, columnValue, null);
                        }
                    }
                }
                else
                {
                    obj = (T)Convert.ChangeType(result[0], typeof(T), CultureInfo.InvariantCulture);
                }

                list.Add(obj);
            }

            return list;
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }
}