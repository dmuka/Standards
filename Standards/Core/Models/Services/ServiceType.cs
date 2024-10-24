using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Services
{
    public class ServiceType : BaseEntity, IEntity<int>
    {
        public static string GetCacheKey()
        {
            return Cache.ServiceTypes;
        }
    }
}
