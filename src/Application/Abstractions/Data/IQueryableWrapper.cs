namespace Application.Abstractions.Data
{
    public interface IQueryableWrapper<T>
    {
        Task<IList<T>> ToListAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    }
}
