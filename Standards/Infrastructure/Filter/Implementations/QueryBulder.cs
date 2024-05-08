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

        private TFilter _filter;

        public QueryBuilder(
            IRepository repository, 
            IQueryBuilderInitializer<T, TFilter> initializer)
        {
            _initializer = initializer;
            _query ??= repository.GetQueryable<T>();
        }

        public IQueryBuilder<T, TFilter> SetFilter(TFilter filterDto)
        {
            _filter = filterDto;

            return this;
        }

        public IQueryBuilder<T, TFilter> Filter()
        {
            foreach (var filter in _initializer.GetFilters(_filter))
            {
                _query = filter.Execute(_query);
            }

            return this;
        }

        public IQueryBuilder<T, TFilter> Sort()
        {
            foreach (var sorting in _initializer.GetSortings(_filter))
            {
                _query = sorting.Execute(_query);
            }

            return this;
        }

        public IQueryBuilder<T, TFilter> Paginate()
        {
            if (!_filter.GetAll())
            {
                _query = _query
                    .Skip((_filter.GetPageNumber() - 1) * _filter.GetItemsPerPage())
                    .Take(_filter.GetItemsPerPage());
            }

            return this;
        }

        public IQueryable<T> GetQuery() => _query;
    }
}