using System.Linq.Expressions;
using System.Reflection;

namespace Standards.Infrastructure.ExpressionTrees;

public static class Expressions
{
    /// <summary>
    /// Get expression tree for lambda: item => item.SearchByProperty.Contains(SearchString)
    /// </summary>
    /// <typeparam name="T">Type of items</typeparam>
    /// <returns>Expression tree <see> <cref>Expression{Func{T, bool}}</cref> </see> </returns>
    public static Expression<Func<T, bool>> GetContains<T>(string searchPropertyName, string searchValue)
    {
        var propertyInfo = typeof(T).GetProperty(searchPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        var lambdaParameter = Expression.Parameter(typeof(T), "item");
        var property = Expression.Property(lambdaParameter, propertyInfo.Name);
        var constant = Expression.Constant(searchValue);
        var containsMethodBody = Expression.Call(
            property, 
            typeof(string).GetMethod("Contains", [ typeof(string) ]),
            constant);

        var lambda = Expression.Lambda<Func<T, bool>>(containsMethodBody, lambdaParameter);

        return lambda;
    }

    /// <summary>
    /// Get expression tree for lambda: item => item.Property
    /// </summary>
    /// <param name="propertyName">Name of the property</param>
    /// <typeparam name="T">Type of items</typeparam>
    /// <returns>Expression tree <see> <cref>Expression{Func{T, object?}}</cref> </see> </returns>
    public static Expression<Func<T, object?>> GetKeySelector<T>(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName);
        Func<T, object?> func = entity => property.GetValue(entity);
        Expression<Func<T, object?>> keySelector = a => func(a);

        return keySelector;
    }
}