namespace Standards.Infrastructure.Filter.Interfaces;

public interface IFilter<TEntity> : IPaginationItem
{
    public IQueryable<TEntity> Execute(IQueryable<TEntity> query);
}