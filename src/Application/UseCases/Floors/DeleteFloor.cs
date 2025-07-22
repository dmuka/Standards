using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Floors;
using MediatR;

namespace Application.UseCases.Floors;

public class DeleteFloor
{
    public class Command(FloorId floorId) : IRequest<Result<int>>
    {
        public FloorId FloorId { get; set; } = floorId;
    }
    
    public class CommandHandler(
        IFloorRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isFloorExist = await repository.ExistsAsync(command.FloorId, cancellationToken);
            
            if (!isFloorExist) return Result.Failure<int>(FloorErrors.NotFound(command.FloorId));
            
            var existingFloor = await repository.GetByIdAsync(command.FloorId, cancellationToken: cancellationToken);
            
            repository.Remove(existingFloor!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}