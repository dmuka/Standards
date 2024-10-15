using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Implementations
{
    public class QueryBuilder<T> : IQueryBuilder<T>
        where T : class
    {
        private IPaginator? _paginator;
        private IFilter<T>? _filter;
        private IFilter<T>? _sorter;

        private IQueryable<T> _query;

        public QueryBuilder(IRepository repository)
        {
            _query =  repository.GetQueryable<T>();
        }

        public IQueryBuilder<T> AddPaginator(IPaginator paginator)
        {
            _paginator = paginator;

            return this;
        }

        public IQueryBuilder<T> AddFilter(IFilter<T> filter)
        {
            _filter = filter;

            return this;
        }

        public IQueryBuilder<T> AddSorter(IFilter<T> sorter)
        {
            _sorter = sorter;

            return this;
        }

        public IQueryBuilder<T> Filter()
        {
            if (_filter is not null)
            {
                _query = _filter.Execute(_query);
            }

            return this;
        }

        public IQueryBuilder<T> Sort()
        {
            if (_sorter is not null)
            {
                _query = _sorter.Execute(_query);
            }

            return this;
        }

        public IQueryBuilder<T> Paginate()
        {
            if (_paginator is not null && !_paginator.GetAll())
            {
                _query = _query
                    .Skip((_paginator.GetPageNumber() - 1) * _paginator.GetItemsPerPage())
                    .Take(_paginator.GetItemsPerPage());
            }

            return this;
        }

        public IQueryable<T> GetQuery() => _query;
    }
}