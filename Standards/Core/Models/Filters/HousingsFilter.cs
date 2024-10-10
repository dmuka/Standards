using System.Linq.Expressions;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.Models.Filters
{
    public class HousingsFilter(Expression<Func<Housing, bool>> func) : Filter<Housing>(func)
    {
        public string SearchQuery { get; set; } = string.Empty;
    }
}
