namespace Standards.Infrastructure.Filter.Interfaces
{
    public interface IQueryBuilder<out T, TFilter>
    {
        IQueryable<T> Filter(TFilter filterDto);

        IQueryable<T> Sort(TFilter filterDto);

        IQueryable<T> Paginate(TFilter filterDto);
    }
}