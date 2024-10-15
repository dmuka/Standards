using Standards.Infrastructure.Filter.Constants;

namespace Standards.Infrastructure.Filter.Implementations;

public record QueryParameters(
    string SearchString = "",
    string SearchBy = "Name",
    string SortBy = "Name",
    bool SortDescending = false,
    int PageNumber = Pagination.FirstPage,
    int ItemsOnPage = Pagination.MinItemsPerPage
    )
{
    // public string SearchString { get; set; } = SearchString;
    // public string SearchBy { get; set; } = SearchBy;
    // public string SortBy { get; set; } = SortBy;
    // public bool SortDescending { get; set; } = SortDescending;
    // public int PageNumber { get; set; } = PageNumber;
    // public int ItemsOnPage { get; set; } = ItemsOnPage;
}