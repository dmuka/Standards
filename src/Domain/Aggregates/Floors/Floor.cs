using Core;
using Domain.Aggregates.Floors.Specifications;
using Domain.Aggregates.Housings;
using Domain.Constants;
using Domain.Models.Housings;
using Housing = Domain.Aggregates.Housings.Housing;

namespace Domain.Aggregates.Floors;

public class Floor : AggregateRoot<FloorId>
{
    public int Number { get; private set; }
    public HousingId HousingId { get; private set; }
    private readonly List<Room> _rooms = [];
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

    private Floor(FloorId floorId, int number, HousingId housingId)
    {
        Id = floorId;
        Number = number;
        HousingId = housingId;
    }

    public static async Task<Result<Floor>> Create(
        int floorNumber, 
        HousingId housingId, 
        IFloorUniqueness uniquenessChecker, 
        CancellationToken cancellationToken = default)
    {
        var floorNumberValidationResult = new FloorNumberMustBeValid(floorNumber).IsSatisfied();
        if (floorNumberValidationResult.IsFailure) return Result.Failure<Floor>(floorNumberValidationResult.Error);

        if (!await uniquenessChecker.IsUniqueAsync(floorNumber, housingId, cancellationToken))
            return Result.Failure<Floor>(FloorErrors.FloorAlreadyExist);

        var floor = new Floor(new FloorId(Guid.CreateVersion7()), floorNumber, housingId);
            
        return Result.Success(floor);
    }

    public static string GetCacheKey()
    {
        return Cache.Floors;
    }
}