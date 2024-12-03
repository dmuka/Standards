using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Standards;

public class Grade : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.Grades;
    }
}