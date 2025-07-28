using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Rooms;
using MediatR;

namespace Application.UseCases.Rooms;

public class DeleteRoom
{
    public class Command(RoomId roomId) : IRequest<Result>
    {
        public RoomId RoomId { get; } = roomId;
    }
    
    public class CommandHandler(
        IRoomRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isRoomExist = await repository.ExistsAsync(command.RoomId, cancellationToken);
            
            if (!isRoomExist) return Result.Failure<int>(RoomErrors.NotFound(command.RoomId));
            
            var existingRoom = await repository.GetByIdAsync(command.RoomId, cancellationToken: cancellationToken);
            
            repository.Remove(existingRoom!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}