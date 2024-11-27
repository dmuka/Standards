using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.MetrologyControl
{
    public class Place : Entity, ICacheable
    {
        public static string GetCacheKey()
        {
            return Cache.Places;
        }
    }
}
