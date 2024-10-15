namespace Standards.Infrastructure.Filter.Interfaces;

public interface IQueryBuilder<T>
    where T : class
{
    IQueryBuilder<T> AddPaginator(IPaginator paginator);
    IQueryBuilder<T> AddFilter(IFilter<T> filter);
    IQueryBuilder<T> AddSorter(IFilter<T> sorter);

    IQueryable<T> Execute();
}