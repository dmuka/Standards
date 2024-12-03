namespace Infrastructure.QueryableWrapper.Interface
{
    public interface IQueryableWrapper<T>
    {
        Task<IList<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    }
}
