using Standards.Infrastructure.Filter.Models;
using System.Text.Json.Serialization;

namespace Standards.Core.Models.DTOs.Filters
{
    public class HousingsFilterDto : PaginationItem
    {
        public string SearchQuery { get; set; }
    }
}
