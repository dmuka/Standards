namespace Standards.Infrastructure.Filter.Interfaces
{
    interface IQueryBuilderItem<T>
    {
        T Execute(T query);
    }
}
