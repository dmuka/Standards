using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models;

public class Quantity : Entity, ICacheable
{
    public IList<Unit> Units { get; set; } = [];
        
    public static string GetCacheKey()
    {
        return Cache.Quantities;
    }
}