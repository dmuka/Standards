namespace Standards.Infrastructure.Filter.Interfaces
{
    public interface IQueryBuilderItem<T>
    {
        T Execute(T query);
    }
}
