using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class Social : Entity, ICacheable
{
    public required string BasePath { get; set; }
    
    public required SocialProfileId ProfileId { get; set; }
    
    public static string GetCacheKey()
    {
        return Cache.Socials;
    }
}