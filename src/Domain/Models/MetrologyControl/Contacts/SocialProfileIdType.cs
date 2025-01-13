using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class SocialProfileIdType : BaseEntity, ICacheable
{
    public required string Value { get; set; }
        
    public static string GetCacheKey()
    {
        return Cache.SocialProfileIds;
    }
}