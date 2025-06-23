using Domain.Constants;
using Domain.Models.Interfaces;
using Domain.Models.MetrologyControl.Contacts;

namespace Domain.Models.MetrologyControl;

public class Place : Entity, ICacheable
{
    public required string Address { get; set; }
    
    public IList<Contact> Contacts { get; set; } = [];
    
    public static string GetCacheKey()
    {
        return Cache.Places;
    }
}