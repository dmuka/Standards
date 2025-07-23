using Application.Abstractions.Data;
using Application.Exceptions;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors.Events.Domain;
using Infrastructure.Exceptions.Enum;
using MediatR;

namespace Application.UseCases.Rooms.EventHandlers;

public sealed class RoomAddedToSectorEventHandler(
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<RoomAddedToSectorEvent>
{
    public async Task Handle(
        RoomAddedToSectorEvent notification, 
        CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(notification.RoomId, cancellationToken);
        if (room is null) throw new StandardsException(
            StatusCodeByError.InternalServerError,
            $"Room {notification.RoomId}: not found ({nameof(RoomAddedToSectorEventHandler)}).",
            "Internal server error"); 
        
        var result = room.ChangeSector(notification.SectorId);
        if (result.IsFailure) throw new StandardsException(
            StatusCodeByError.InternalServerError,
            $"Room {room.Id}: change sector {notification.Id} value error ({nameof(RoomAddedToSectorEventHandler)}, {result.Error.Description}).",
            "Internal server error"); 
        
        roomRepository.Update(room);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}