using Domain.Constants;
using Domain.Models.Interfaces;
using Domain.Models.Persons;
using Domain.Models.Standards;

namespace Domain.Models.Services;

public class ServiceJournalItem : Entity, ICacheable
{
    public required Standard Standard { get; set; }
    public required Person Person { get; set; }
    public required Service Service { get; set; }
    public required DateTime Date { get; set; }

    public static string GetCacheKey()
    {
        return Cache.ServiceJournal;
    }
}