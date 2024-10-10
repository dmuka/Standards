using System.Linq.Expressions;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Filter.Models;

namespace Standards.Core.Models.Filters
{
    public class RoomsFilter(Expression<Func<Room, bool>> func) : Filter<Room>(func)
    {
        public string SearchQuery { get; set; } = string.Empty;
    }
}
