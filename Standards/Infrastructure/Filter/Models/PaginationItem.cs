namespace Standards.Infrastructure.Filter.Models
{
    public class PaginationItem
    {
        private const int DefaultItemsPerPage = 10;
        private const int DefaultPageNumber = 1;
        private const int AllPages = 0;

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public int GetPageNumber()
        {
            return PageNumber == default ? DefaultPageNumber : PageNumber;
        }

        public int GetItemsPerPage()
        {
            return ItemsPerPage == default ? DefaultItemsPerPage : ItemsPerPage;
        }

        public bool GetAll()
        {
            return ItemsPerPage == AllPages;
        }
    }
}
