using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Departments
{
    public class Department : Entity, ICacheable
    {
        public IList<Sector> Sectors { get; set; } = [];

        public static string GetCacheKey()
        {
            return Cache.Departments;
        }
    }
}
