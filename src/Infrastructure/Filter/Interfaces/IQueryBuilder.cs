using Infrastructure.Filter.Implementations;

namespace Infrastructure.Filter.Interfaces;

public interface IQueryBuilder<T> where T : class
{
    IQueryable<T> Execute(QueryParameters parameters);
}