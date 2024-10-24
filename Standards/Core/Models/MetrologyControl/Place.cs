using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.MetrologyControl
{
    public class Place : BaseEntity, IEntity<int>
    {
        public static string GetCacheKey()
        {
            return Cache.Places;
        }
    }
}
