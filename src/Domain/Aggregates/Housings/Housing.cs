using Core;
using Domain.Aggregates.Rooms;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Housings;

public class Housing : AggregateRoot<HousingId>, ICacheable
{
    public required Address Address { get; set; } = null!;
    public required FloorCount FloorsCount { get; set; }
    public IReadOnlyCollection<RoomId> RoomIds => _roomIds.AsReadOnly();
    private List<RoomId> _roomIds = [];

    public static string GetCacheKey()
    {
        return Cache.Housings;
    }
}