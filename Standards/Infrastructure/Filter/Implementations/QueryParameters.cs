using Standards.Infrastructure.Filter.Constants;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Infrastructure.Filter.Implementations;

public record QueryParameters(
    string SearchString = "",
    FilterBy SearchBy = FilterBy.Name,
    SortBy SortBy = SortBy.Name,
    bool SortDescending = false,
    int PageNumber = Pagination.FirstPage,
    int ItemsOnPage = Pagination.MinItemsPerPage
    );