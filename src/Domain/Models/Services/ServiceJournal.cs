using Domain.Models.Persons;
using Domain.Models.Standards;

namespace Domain.Models.Services;

public class ServiceJournal
{
    public int Id { get; set; }
    public Standard Standard { get; set; }
    public Person Person { get; set; }
    public Service Service { get; set; }
    public DateTime Date { get; set; }
    public string Comments { get; set; } = null!;
}