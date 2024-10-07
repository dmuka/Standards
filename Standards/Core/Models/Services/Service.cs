using Standards.Core.Models;

namespace Standards.Core.Models.Services
{
    public class Service : BaseEntity
    {
        public ServiceType ServiceType { get; set; }
        public IList<Material> Materials { get; set; } = new List<Material>();
        public IList<Quantity> MaterialsQuantities { get; set; } = new List<Quantity>();
        public IList<Unit> MaterialsUnits { get; set; } = new List<Unit>();
    }
}
