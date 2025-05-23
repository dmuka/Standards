using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.ExpressionTrees;

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
        var property = Expression.Property(lambdaParameter, propertyInfo?.Name ?? "");
        var constant = Expression.Constant(searchValue);
        var containsMethodBody = Expression.Call(
            property,
            GetMethodInfo("Contains", typeof(string)),
            constant);

        var lambda = Expression.Lambda<Func<T, bool>>(containsMethodBody, lambdaParameter);

        return lambda;
    }

    public static Expression<Func<T, object?>> GetKeySelector<T>(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName);
        if (property is null) throw new ArgumentException($"Property {propertyName} not found in type {typeof(T)}");
        Func<T, object?> func = entity => property.GetValue(entity);
        Expression<Func<T, object?>> keySelector = a => func(a);

        return keySelector;
    }

    private static MethodInfo GetMethodInfo(string methodName, Type type)
    {
        var methodInfo = type.GetMethod(methodName, [type]);
        
        if (methodInfo is null) throw new ArgumentException($"Method {methodName} of the type {type} not found");
        
        return methodInfo;
    }
}