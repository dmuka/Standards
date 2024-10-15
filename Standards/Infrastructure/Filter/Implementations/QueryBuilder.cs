using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Implementations
{
    public class QueryBuilder<T>(IRepository repository) : IQueryBuilder<T>
        where T : class
    {
        private IQueryable<T> _query = repository.GetQueryable<T>();

        public IQueryable<T> Execute(QueryParameters parameters)
        {
            if (!string.IsNullOrEmpty(parameters.SearchBy))
            {
                var propertyInfo = typeof(T).GetProperty(parameters.SearchBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        _query = _query.Where(item => ((string)propertyInfo.GetValue(item)).Contains(parameters.SearchBy));
                    }
                    else if (propertyInfo.PropertyType == typeof(int))
                    {
                        if (int.TryParse(parameters.SearchBy, out var intValue))
                        {
                            _query = _query.Where(item => (int)propertyInfo.GetValue(item) == intValue);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(double))
                    {
                        if (double.TryParse(parameters.SearchBy, out var decimalValue))
                        {
                            _query = _query.Where(item => (double)propertyInfo.GetValue(item) == decimalValue);
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(parameters.SearchBy, out var dateTimeValue))
                        {
                            _query = _query.Where(item => ((DateTime)propertyInfo.GetValue(item)).Date == dateTimeValue.Date);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                _query = parameters.SortDescending
                    ? _query.OrderByDescending(entity => EF.Property<object>(entity, parameters.SortBy))
                    : _query.OrderBy(entity => EF.Property<object>(entity, parameters.SortBy));
            }
        
            _query = _query
                .Skip((parameters.PageNumber - 1) * parameters.ItemsOnPage)
                .Take(parameters.ItemsOnPage);

            return _query;
        }
    }
}