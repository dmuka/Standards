using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Departments
{
    public class Department : BaseEntity, IEntity<int>
    {
        public IList<Sector> Sectors { get; set; } = new List<Sector>();
        public IList<Housing> Housings { get; set; } = new List<Housing>();

        public static string GetCacheKey()
        {
            return Cache.Departments;
        }
    }
}
