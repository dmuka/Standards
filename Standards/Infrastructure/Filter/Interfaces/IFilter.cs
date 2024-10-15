namespace Standards.Infrastructure.Filter.Interfaces;

public interface IFilter<TEntity>
{
    public IQueryable<TEntity> Execute(IQueryable<TEntity> query);
}