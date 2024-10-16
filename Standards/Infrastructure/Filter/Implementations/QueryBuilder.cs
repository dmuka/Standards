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

            if (parameters.SortBy != SortBy.None)
            {
                _query = parameters.SortDescending
                    ? _query.OrderByDescending(entity => EF.Property<object>(entity, Enum.GetName(parameters.SortBy.Value)))
                    : _query.OrderBy(entity => EF.Property<object>(entity, Enum.GetName(parameters.SortBy.Value)));
            }
        
            _query = _query
                .Skip((parameters.PageNumber - 1) * parameters.ItemsOnPage)
                .Take(parameters.ItemsOnPage);

            return _query;
        }
    }
}