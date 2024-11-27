using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models;

public class Quantity : Entity, ICacheable
{
    public IList<Unit> Units { get; set; } = [];
        
    public static string GetCacheKey()
    {
        return Cache.Quantities;
    }
}