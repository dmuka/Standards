using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Services;

public class Service : Entity, ICacheable
{
    public required ServiceType ServiceType { get; set; }
    public IList<Material> Materials { get; set; } = [];
    public IList<Quantity> MaterialsQuantities { get; set; } = [];
    public IList<Unit> MaterialsUnits { get; set; } = [];

    public static string GetCacheKey()
    {
        return Cache.Services;
    }
}