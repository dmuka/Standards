using Core.Results;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class AddRoom
{
    public class Command(SectorId sectorId, RoomId roomId) : IRequest<Result>
    {
        public required RoomId SectorId { get; set; }
        public required RoomId RoomId { get; set; }
    }

    public class CommandHandler(
        ISectorRepository sectorRepository, 
        IRoomRepository roomRepository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var sector = await sectorRepository.GetByIdAsync(request.SectorId, cancellationToken);
            if (sector is null) return Result.Failure(SectorErrors.NotFound(request.SectorId));
            
            var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
            if (room is null) return Result.Failure(RoomErrors.NotFound(request.RoomId));
            if (room.SectorId is not null) return Result.Failure(SectorErrors.ThisRoomAlreadySetForAnotherSector);
            
            var result = sector.AddRoom(request.RoomId);
            
            return result.IsFailure 
                ? Result.Failure(result.Error) 
                : Result.Success();
        }
    }
}