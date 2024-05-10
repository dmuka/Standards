namespace Standards.Infrastructure.Filter.Interfaces
{
    public interface IQueryBuilderInitializer<T, TFilter>
    {
        IEnumerable<IQueryBuilderItem<IQueryable<T>>> GetFilters(TFilter filterDto);

        IEnumerable<IQueryBuilderItem<IQueryable<T>>> GetSortings(TFilter filterDto);
    }
}
