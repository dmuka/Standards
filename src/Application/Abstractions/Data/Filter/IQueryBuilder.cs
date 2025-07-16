using Application.Abstractions.Data.Filter.Models;

namespace Application.Abstractions.Data.Filter;

public interface IQueryBuilder<T> where T : class
{
    IQueryable<T> Execute(QueryParameters parameters);
}