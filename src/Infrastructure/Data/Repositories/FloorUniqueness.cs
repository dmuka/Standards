using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data.Repositories.Interfaces;

namespace Infrastructure.Data.Repositories;

public class FloorUniqueness(IRepository repository) : IFloorUniqueness
{
    public async Task<bool> IsUniqueAsync(
        int floorNumber, 
        HousingId housingId, 
        CancellationToken cancellationToken = default)
    {
        return await repository.ExistsAsync<Floor>(floor => 
            floor.HousingId == housingId && floor.Number == floorNumber, cancellationToken);
    }
}