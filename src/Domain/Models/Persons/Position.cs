using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Persons;

public class Position : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.Positions;
    }
}