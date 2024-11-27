using Standards.Infrastructure.Filter.Constants;

namespace Standards.Core.Models;

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

    public int TotalCount { get; set; }

    public int CurrentPageSize { get; set; }

    public int CurrentPageNumber { get; set; }

    public int ItemsPerPage { get; set; }

    public bool IsLastPage => CurrentPageNumber * ItemsPerPage >= TotalCount || CurrentPageSize == TotalCount;

    public static PaginatedListModel<TEntity> ApplyPagination<TEntity>(
        IEnumerable<TEntity> entities,
        int pageNumber,
        int itemsOnPage)
    {
        PaginatedListModel<TEntity> result = null;
                
        if (entities is not null)
        {
            result = new PaginatedListModel<TEntity>(entities, pageNumber, itemsOnPage);
        }

        return result;
    }
}