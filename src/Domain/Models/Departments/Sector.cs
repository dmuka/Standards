using Domain.Constants;
using Domain.Models.Housings;
using Domain.Models.Interfaces;
using Domain.Models.Persons;

namespace Domain.Models.Departments;

public class Sector : Entity, ICacheable
{
    public required Department Department { get; set; }
    public IList<Room> Rooms { get; set; } = [];
    public IList<Workplace> Workplaces { get; set; } = [];
    public IList<Person> Persons { get; set; } = [];

    public static string GetCacheKey()
    {
        return Cache.Sectors;
    }
}