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
    public static Expression<Func<T, bool>> GetContainsLambda<T>(string searchPropertyName, string searchValue)
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
}