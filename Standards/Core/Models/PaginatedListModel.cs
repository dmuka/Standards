namespace Standards.Core.Models
{
    public class PaginatedListModel<T>
    {
        public PaginatedListModel(
            IEnumerable<T> items, 
            int currentPageNumber, 
            int currentPageSize, 
            int itemsPerPage)
        {
            Items = items;
            CurrentPageNumber = currentPageNumber;
            CurrentPageSize = currentPageSize;
            ItemsPerPage = itemsPerPage;
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
