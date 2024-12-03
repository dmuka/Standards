using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl;

public class Place : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.Places;
    }
}