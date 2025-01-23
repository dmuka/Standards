using Domain.Models.Constants;

namespace Domain.Models;

public class PaginatedListModel<T>
{
    public PaginatedListModel(
        IEnumerable<T> items, 
        int currentPageNumber,
        int itemsPerPage)
    {
        var enumerable = items as T[] ?? items.ToArray();
        Items = enumerable;
        CurrentPageNumber = currentPageNumber < 1 ? Pagination.FirstPage : currentPageNumber;
        CurrentPageSize = enumerable.Length;
        ItemsPerPage = itemsPerPage < 1 ? Pagination.MinItemsPerPage : itemsPerPage;
        TotalCount = enumerable.Length;
    }

    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; }

    public int CurrentPageSize { get; }

    public int CurrentPageNumber { get; set; }

    public int ItemsPerPage { get; set; }

    public bool IsLastPage => CurrentPageNumber * ItemsPerPage >= TotalCount || CurrentPageSize == TotalCount;

    public static PaginatedListModel<TEntity> ApplyPagination<TEntity>(
        IEnumerable<TEntity> entities,
        int pageNumber,
        int itemsOnPage)
    {
        var result =  new PaginatedListModel<TEntity>(entities, pageNumber, itemsOnPage);

        return result;
    }
}