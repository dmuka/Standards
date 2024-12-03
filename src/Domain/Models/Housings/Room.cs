using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Interfaces;
using Domain.Models.Persons;

namespace Domain.Models.Housings;

public class Room : Entity, ICacheable
{
    public required Housing Housing { get; set; }
    public required int Floor { get; set; }
    public required double Length { get; set; }
    public required double Height { get; set; }
    public required double Width { get; set; }
    public IList<Workplace> WorkPlaces { get; set; } = [];
    public IList<Person> Persons { get; set; } = [];
    public required Sector Sector { get; set; }

    public static string GetCacheKey()
    {
        return Cache.Rooms;
    }
}