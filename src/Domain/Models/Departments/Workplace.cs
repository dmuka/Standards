using Domain.Constants;
using Domain.Models.Housings;
using Domain.Models.Interfaces;
using Domain.Models.Persons;

namespace Domain.Models.Departments;

public class Workplace : Entity, ICacheable
{
    public required Room? Room { get; set; }
    public required Person? Responsible { get; set; }
    public string? ImagePath { get; set; }

    public static string GetCacheKey()
    {
        return Cache.Workplaces;
    }
}