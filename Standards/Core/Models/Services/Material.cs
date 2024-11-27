using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Services
{
    public class Material : Entity, ICacheable
    {
        public Unit Unit { get; set; }

        public static string GetCacheKey()
        {
            return Cache.Materials;
        }
    }
}
