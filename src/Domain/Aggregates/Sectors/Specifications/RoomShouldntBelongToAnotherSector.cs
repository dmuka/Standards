using Core;
using Core.Results;
using Domain.Aggregates.Rooms;

namespace Domain.Aggregates.Sectors.Specifications;

public class RoomShouldNotBelongToAnotherSector(RoomId roomId, IRoomRepository repository) : IAsyncSpecification
{
    public async Task<Result> IsSatisfiedAsync(CancellationToken cancellationToken)
    {
        var room = await repository.GetByIdAsync(roomId, cancellationToken);
        if (room is null) return Result.Failure(RoomErrors.NotFound(roomId));

        return room.SectorId is null ? Result.Failure(SectorErrors.ThisRoomAlreadySetForAnotherSector) : Result.Success();
    }
}