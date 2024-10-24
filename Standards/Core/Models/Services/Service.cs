using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Services
{
    public class Service : BaseEntity, IEntity<int>
    {
        public ServiceType ServiceType { get; set; }
        public IList<Material> Materials { get; set; } = new List<Material>();
        public IList<Quantity> MaterialsQuantities { get; set; } = new List<Quantity>();
        public IList<Unit> MaterialsUnits { get; set; } = new List<Unit>();

        public static string GetCacheKey()
        {
            return Cache.Services;
        }
    }
}
