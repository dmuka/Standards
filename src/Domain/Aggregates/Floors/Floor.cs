using Core;
using Core.Results;
using Domain.Aggregates.Floors.Specifications;
using Domain.Aggregates.Housings;
using Domain.Constants;
using Domain.Models.Interfaces;
using Room = Domain.Models.Housings.Room;

namespace Domain.Aggregates.Floors;

public class Floor : AggregateRoot<FloorId>, ICacheable
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

    public static Result<Floor> Create(
        int floorNumber, 
        Guid housingId, 
        Guid? floorId = null)
    {
        var validationResults = ValidateFloorDetails(floorNumber);
        if (validationResults.Length != 0)
            return Result<Floor>.ValidationFailure(ValidationError.FromResults(validationResults));

        var floor = new Floor(
            floorId is null ? new FloorId(Guid.CreateVersion7()) : new FloorId(floorId.Value), 
            floorNumber, 
            new HousingId(housingId));
            
        return Result.Success(floor);
    }

    public static string GetCacheKey()
    {
        return Cache.Floors;
    }

    /// <summary>
    /// Validates floor details.
    /// </summary>
    private static Result[] ValidateFloorDetails(int floorNumber)
    {
        var validationResults = new []
        {
            new FloorNumberMustBeGreaterThanZero(floorNumber).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}