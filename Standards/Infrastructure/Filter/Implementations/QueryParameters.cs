using Standards.Infrastructure.Filter.Constants;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Infrastructure.Filter.Implementations;

public class QueryParameters(
    string searchString = "",
    FilterBy? searchBy = FilterBy.Name,
    SortBy? sortBy = SortBy.Name,
    bool sortDescending = false,
    int pageNumber = Pagination.FirstPage,
    int itemsOnPage = Pagination.MinItemsPerPage
)
{
    public string SearchString { get; set; } = searchString;
    public FilterBy? SearchBy { get; set; } = searchBy;
    public SortBy? SortBy { get; set; } = sortBy;
    public bool SortDescending { get; set; } = sortDescending;
    public int PageNumber { get; set; } = pageNumber;
    public int ItemsOnPage { get; set; } = itemsOnPage;
}