using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Departments;

public class Department : Entity, ICacheable
{
    public IList<Sector> Sectors { get; set; } = [];

    public static string GetCacheKey()
    {
        return Cache.Departments;
    }
}