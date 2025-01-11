using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class Email : BaseEntity, ICacheable
{
    public required string Value { get; set; }
    
    public required Contact Contact { get; set; }
    
    public static string GetCacheKey()
    {
        return Cache.Emails;
    }
}