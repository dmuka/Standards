using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models;

public class Quantity : BaseEntity, IEntity<int>
{
    public IList<Unit> Units { get; set; } = null!;
        
    public static string GetCacheKey()
    {
        return Cache.Quantities;
    }
}