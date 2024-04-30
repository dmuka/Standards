using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Infrastructure.Filter.Implementations
{
    class QueryBuilder<T, TFilter> : IQueryBuilder<T, TFilter>
        where TFilter : PaginationItem
        where T : class
    {
        private readonly IQueryBuilderInitializer<T, TFilter> _initializer;

        private IQueryable<T> _query;

        public QueryBuilder(
            IRepository repository, 
            IQueryBuilderInitializer<T, TFilter> initializer)
        {
            _initializer = initializer;
            _query ??= repository.GetQueryable<T>();
        }

        public IQueryable<T> Filter(TFilter filterDto)
        {
            foreach (var filter in _initializer.GetFilters(filterDto))
            {
                _query = filter.Execute(_query);
            }

            return _query;
        }

        public IQueryable<T> Sort(TFilter filterDto)
        {
            foreach (var sorting in _initializer.GetSortings(filterDto))
            {
                _query = sorting.Execute(_query);
            }

            return _query;
        }

        public IQueryable<T> Paginate(TFilter filterDto)
        {
            if (!filterDto.GetAll())
            {
                _query = _query
                    .Skip((filterDto.GetPageNumber() - 1) * filterDto.GetItemsPerPage())
                    .Take(filterDto.GetItemsPerPage());
            }

            return _query;
        }
    }
}