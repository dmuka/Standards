using Domain.Aggregates.Housings;

namespace Domain.Aggregates.Floors;

public interface IFloorUniqueness
{
    Task<bool> IsUniqueAsync(int floorNumber, HousingId housingId, CancellationToken cancellationToken = default);
}