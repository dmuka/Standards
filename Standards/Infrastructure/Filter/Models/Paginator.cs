using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.Infrastructure.Filter.Models
{
    public class Paginator(int pageNumber, int itemsPerPage) : IPaginator
    {
        public int PageNumber { get; set; } = pageNumber;

        public int ItemsPerPage { get; set; } = itemsPerPage;
    }
}
