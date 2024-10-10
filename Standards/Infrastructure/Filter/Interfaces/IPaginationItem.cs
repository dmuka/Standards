using Standards.Infrastructure.Filter.Constants;

namespace Standards.Infrastructure.Filter.Interfaces;

public interface IPaginationItem
{
    private const int AllPages = 0;

    public int PageNumber { get; set; }

    public int ItemsPerPage { get; set; }

    public int GetPageNumber()
    {
        return PageNumber < Pagination.FirstPage ? Pagination.FirstPage : PageNumber;
    }

    public int GetItemsPerPage()
    {
        return ItemsPerPage < Pagination.MinItemsPerPage ? Pagination.MinItemsPerPage : ItemsPerPage;
    }

    public bool GetAll()
    {
        return ItemsPerPage == AllPages;
    }
}