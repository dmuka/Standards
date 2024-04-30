namespace Standards.Infrastructure.Filter.Interfaces
{
    interface IQueryBuilderInitializer<T, TFilter>
    {
        IEnumerable<IQueryBuilderItem<IQueryable<T>>> GetFilters(TFilter filterDto);

        IEnumerable<IQueryBuilderItem<IQueryable<T>>> GetSortings(TFilter filterDto);
    }
}
