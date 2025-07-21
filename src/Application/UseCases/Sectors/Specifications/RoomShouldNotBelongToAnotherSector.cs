using Core;
using Core.Results;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;

namespace Application.UseCases.Sectors.Specifications;

public class RoomShouldNotBelongToAnotherSector(Room room) : ISpecification
{
    public Result IsSatisfied()
    {
        return room.SectorId is not null 
            ? Result.Failure(SectorErrors.ThisRoomAlreadySetForAnotherSector) 
            : Result.Success();
    }
}