using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Infrastructure.Filter.Interfaces;

public interface IQueryBuilder<T>
    where T : class
{
    IQueryable<T> Execute(QueryParameters parameters);
}