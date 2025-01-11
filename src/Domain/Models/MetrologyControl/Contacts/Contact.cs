using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.MetrologyControl.Contacts;

public class Contact : Entity, ICacheable
{
    public required Place Place { get; set; }
    public IList<Email> Emails { get; set; } = [];
    public IList<Phone> Phones { get; set; } = [];
    public IList<Social> Socials { get; set; } = [];
    
    public static string GetCacheKey()
    {
        return Cache.Places;
    }
}