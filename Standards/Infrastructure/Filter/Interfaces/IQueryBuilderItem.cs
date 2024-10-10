namespace Standards.Infrastructure.Filter.Interfaces;

public interface IQueryBuilderItem<TEntity>
{
    IQueryable<TEntity> Execute(IQueryable<TEntity> query);
}
