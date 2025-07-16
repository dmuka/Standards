using Core;
using Domain.Aggregates.Floors.Specifications;
using Domain.Aggregates.Housings;
using Domain.Constants;
using Domain.Models.Interfaces;
using Room = Domain.Models.Housings.Room;

namespace Domain.Aggregates.Floors;

public class Floor : AggregateRoot, ICacheable
{
    public int Number { get; private set; }
    public HousingId HousingId { get; private set; } = null!;
    private readonly List<Room> _rooms = [];
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();
    
    protected Floor()
    {
    }

    private Floor(FloorId floorId, int number, HousingId housingId)
    {
        Id = floorId;
        Number = number;
        HousingId = housingId;
    }

    public static Result<Floor> Create(int floorNumber, HousingId housingId, FloorId? floorId = null)
    {
        var floorNumberValidationResult = new FloorNumberMustBeValid(floorNumber).IsSatisfied();
        if (floorNumberValidationResult.IsFailure) return Result.Failure<Floor>(floorNumberValidationResult.Error);

        var floor = new Floor(floorId ?? new FloorId(Guid.CreateVersion7()), floorNumber, housingId);
            
        return Result.Success(floor);
    }

    public static string GetCacheKey()
    {
        return Cache.Floors;
    }
}