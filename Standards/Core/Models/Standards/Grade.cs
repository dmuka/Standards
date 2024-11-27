using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.Standards;

public class Grade : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.Grades;
    }
}