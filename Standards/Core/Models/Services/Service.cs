using Standards.Core.Models;

namespace Standards.Core.Models.Services
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ServiceType ServiceType { get; set; }
        public IList<Material> Materials { get; set; } = new List<Material>();
        public IList<Quantity> MaterialsQuantities { get; set; } = new List<Quantity>();
        public IList<Unit> MaterialsUnits { get; set; } = new List<Unit>();
        public string Comments { get; set; } = null!;
    }
}
