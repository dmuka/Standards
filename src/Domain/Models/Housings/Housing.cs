using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Housings;

public class Housing : Entity, ICacheable
{
    [MaxLength(Lengths.Address)]
    public required string Address { get; set; } = null!;
    public required int FloorsCount { get; set; }
    public IList<Room> Rooms { get; set; } = [];

    public static string GetCacheKey()
    {
        return Cache.Housings;
    }
}