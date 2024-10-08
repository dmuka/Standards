﻿using Standards.Infrastructure.Filter.Constants;

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
            var enumerable = items as T[] ?? items.ToArray();
            Items = enumerable;
            CurrentPageNumber = currentPageNumber < 1 ? Pagination.FirstPage : currentPageNumber;
            CurrentPageSize = currentPageSize;
            ItemsPerPage = itemsPerPage < 1 ? Pagination.MinItemsPerPage : itemsPerPage;
            TotalCount = enumerable.Length;
        }

        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPageSize { get; set; }

        public int CurrentPageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public bool IsLastPage => CurrentPageNumber * ItemsPerPage >= TotalCount || CurrentPageSize == TotalCount;
    }
}
