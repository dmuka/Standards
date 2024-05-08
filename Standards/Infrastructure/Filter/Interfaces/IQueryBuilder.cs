namespace Standards.Infrastructure.Filter.Interfaces
{
    public interface IQueryBuilder<out T, TFilter>
    {
        IQueryBuilder<T, TFilter> SetFilter(TFilter filterDto);

        IQueryBuilder<T, TFilter> Filter();

        IQueryBuilder<T, TFilter> Sort();

        IQueryBuilder<T, TFilter> Paginate();

        IQueryable<T> GetQuery();
    }
}