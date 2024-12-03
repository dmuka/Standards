using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Services;

public class ServiceType : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.ServiceTypes;
    }
}