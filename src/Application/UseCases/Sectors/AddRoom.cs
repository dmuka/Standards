using Application.Abstractions.Data;
using Application.UseCases.Sectors.Specifications;
using Core.Results;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

/// <summary>
/// Represents the use case for adding a room to a sector.
/// </summary>
public class AddRoom
{
    /// <summary>
    /// Command to add a room to a sector.
    /// </summary>
    /// <param name="sectorId">The identifier of the sector.</param>
    /// <param name="roomId">The identifier of the room.</param>
    public class Command(SectorId sectorId, RoomId roomId) : IRequest<Result>
    {
        /// <summary>
        /// Gets the identifier of the sector.
        /// </summary>
        public SectorId SectorId { get; } = sectorId;

        /// <summary>
        /// Gets the identifier of the room.
        /// </summary>
        public RoomId RoomId { get; } = roomId;
    }

    /// <summary>
    /// Handles the command to add a room to a sector.
    /// </summary>
    public class CommandHandler(
        ISectorRepository sectorRepository, 
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        /// <summary>
        /// Handles the process of adding a room to a sector.
        /// </summary>
        /// <param name="request">The command containing the sector and room identifiers.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A result indicating success or failure of the operation.</returns>
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var sector = await sectorRepository.GetByIdAsync(request.SectorId, cancellationToken);
            if (sector is null) return Result.Failure(SectorErrors.NotFound(request.SectorId));
            
            var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
            if (room is null) return Result.Failure(RoomErrors.NotFound(request.RoomId));
            
            var specificationResult = new RoomShouldNotBelongToAnotherSector(room).IsSatisfied();
            if (specificationResult.IsFailure) return Result.Failure(specificationResult.Error);
            
            var result = sector.AddRoom(request.RoomId);
            if (result.IsFailure) return Result.Failure(result.Error);
            sectorRepository.Update(sector);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}