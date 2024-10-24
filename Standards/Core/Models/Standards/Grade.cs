using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Standards
{
    public class Grade : BaseEntity, IEntity<int>
    {
        public static string GetCacheKey()
        {
            return Cache.Grades;
        }
    }
}
