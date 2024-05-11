namespace Standards.Core.Models
{
    public class PaginatedListModel<T>
    {
        private const int DefaultItemsPerPage = 10;
        private const int DefaultPageNumber = 1;

        public PaginatedListModel(
            IEnumerable<T> items, 
            int currentPageNumber, 
            int currentPageSize, 
            int itemsPerPage)
        {
            Items = items;
            CurrentPageNumber = currentPageNumber < 1 ? DefaultPageNumber : currentPageNumber;
            CurrentPageSize = currentPageSize;
            ItemsPerPage = itemsPerPage < 1 ? DefaultItemsPerPage : itemsPerPage;
            TotalCount = items.Count();
        }

        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPageSize { get; set; }

        public int CurrentPageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public bool IsLastPage => CurrentPageNumber * ItemsPerPage >= TotalCount || CurrentPageSize == TotalCount;
    }
}
