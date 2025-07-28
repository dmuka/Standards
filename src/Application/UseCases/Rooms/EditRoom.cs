using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Rooms;
using MediatR;

namespace Application.UseCases.Rooms;

public class EditRoom
{
    public class Command(RoomDto2 room) : IRequest<Result>
    {
        public RoomDto2 RoomDto { get; } = room;
    }

    public class CommandHandler(
        IRoomRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isRoomExist = await repository.ExistsAsync(command.RoomDto.Id, cancellationToken);
            
            if (!isRoomExist) return Result.Failure(RoomErrors.NotFound(command.RoomDto.Id));
            
            var existingRoom = await repository.GetByIdAsync(command.RoomDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingRoom!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}