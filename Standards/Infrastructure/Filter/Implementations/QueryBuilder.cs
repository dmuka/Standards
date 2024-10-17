using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.ExpressionTrees;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Infrastructure.Filter.Implementations
{
    public class QueryBuilder<T>(IRepository repository) : IQueryBuilder<T>
        where T : class
    {
        private IQueryable<T> _query = repository.GetQueryable<T>();

        public IQueryable<T> Execute(QueryParameters parameters)
        {
            if (parameters.SearchBy != FilterBy.None)
            {
                var expression = Expressions.GetContainsLambda<T>(Enum.GetName(parameters.SearchBy.Value), parameters.SearchString);
                
                _query = _query.Where(expression);
            }
            
            var propertyName = Enum.GetName(parameters.SortBy.Value);
            var property = typeof(T).GetProperty(propertyName);
            Func<T, object?> func = entity => property.GetValue(entity);
            Expression<Func<T, object?>> sortExpression = a => func(a);
            //var sortExpression = Expression.Lambda<Func<T, object?>>(Expression.Call(func.Method));
            
            if (parameters.SortBy != SortBy.None)
            {
                _query = parameters.SortDescending
                    ? _query.OrderByDescending(sortExpression)
                    : _query.OrderBy(sortExpression);
            }
        
            _query = _query
                .Skip((parameters.PageNumber - 1) * parameters.ItemsOnPage)
                .Take(parameters.ItemsOnPage);

            return _query;
        }
    }
}