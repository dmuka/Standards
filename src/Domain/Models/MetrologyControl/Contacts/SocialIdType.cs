using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class SocialProfileId : Entity, ICacheable
{
    public required string Name { get; set; }
    // PhoneNumber,
    // Email,
    // UserName
        
    public static string GetCacheKey()
    {
        throw new NotImplementedException();
    }
}