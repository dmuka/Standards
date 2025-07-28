using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Rooms;
using MediatR;

namespace Application.UseCases.Rooms;

public class AddRoom
{
    public class Command(RoomDto2 room) : IRequest<Result<Room>>
    {
        public RoomDto2 RoomDto { get; } = room;
    };

    public class CommandHandler(
        IRoomRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Room>>
    {
        public async Task<Result<Room>> Handle(Command command, CancellationToken cancellationToken)
        {
            var roomCreationResult = Room.Create(
                command.RoomDto.Length, 
                command.RoomDto.Height, 
                command.RoomDto.Width, 
                command.RoomDto.Id,
                command.RoomDto.Comments);

            if (roomCreationResult.IsFailure) return Result.Failure<Room>(roomCreationResult.Error);

            var room = roomCreationResult.Value;
            
            await repository.AddAsync(room, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(room);
        }
    }
}