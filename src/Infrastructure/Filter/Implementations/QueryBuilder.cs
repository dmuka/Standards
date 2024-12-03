using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.ExpressionTrees;
using Infrastructure.Filter.Interfaces;
using Infrastructure.Filter.Models;

namespace Infrastructure.Filter.Implementations
{
    public class QueryBuilder<T>(IRepository repository) : IQueryBuilder<T>
        where T : class
    {
        private IQueryable<T> _query = repository.GetQueryable<T>();

        public IQueryable<T> Execute(QueryParameters parameters)
        {
            if (parameters.SearchBy != FilterBy.None)
            {
                var expression = Expressions.GetContains<T>(
                    Enum.GetName(parameters.SearchBy.Value), 
                    parameters.SearchString);
                
                _query = _query.Where(expression);
            }
            
            var keySelector = Expressions.GetKeySelector<T>(Enum.GetName(parameters.SortBy.Value));
            
            if (parameters.SortBy != SortBy.None)
            {
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