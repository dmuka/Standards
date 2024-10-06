using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.Models.DTOs.Filters
{
    public class HousingsFilterDto : PaginationItem
    {
        public string SearchQuery { get; set; } = string.Empty;
    }
}
