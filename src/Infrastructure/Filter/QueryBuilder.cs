using Application.Abstractions.Data;
using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Infrastructure.ExpressionTrees;
using Infrastructure.Filter.Models;

namespace Infrastructure.Filter.Implementations
{
    public class QueryBuilder<T>(IRepository repository) : IQueryBuilder<T>
        where T : class
    {
        private IQueryable<T> _query = repository.GetQueryable<T>();

        public IQueryable<T> Execute(QueryParameters parameters)
        {
            if (parameters.SearchBy is not null && parameters.SearchBy != FilterBy.None)
            {
                var expression = Expressions.GetContains<T>(
                    Enum.GetName(parameters.SearchBy.Value) ?? "", 
                    parameters.SearchString);
                
                _query = _query.Where(expression);
            }

            if (parameters.SortBy is not null && parameters.SortBy != SortBy.None)
            {
                var keySelector = Expressions.GetKeySelector<T>(Enum.GetName(parameters.SortBy.Value) ?? "");
                
                _query = parameters.SortDescending
                    ? _query.OrderByDescending(keySelector)
                    : _query.OrderBy(keySelector);
            }

            _query = _query
                .Skip((parameters.PageNumber - 1) * parameters.ItemsOnPage)
                .Take(parameters.ItemsOnPage);

            return _query;
        }
    }
}