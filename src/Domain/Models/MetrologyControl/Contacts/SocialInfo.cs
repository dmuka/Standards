using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class SocialInfo : Entity, ICacheable
{
    public required string BasePath { get; set; }
    
    public required SocialProfileIdType ProfileIdType { get; set; }
    
    public static string GetCacheKey()
    {
        return Cache.SocialProperties;
    }
}