using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Floors;
using MediatR;

namespace Application.UseCases.Floors;

public class EditFloor
{
    public class Command(FloorDto floor) : IRequest<Result>
    {
        public FloorDto FloorDto { get; set; } = floor;
    }
    
    public class CommandHandler(IFloorRepository repository, IFloorUniqueness floorUniqueness) 
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isFloorExist = await repository.ExistsAsync(command.FloorDto.FloorId, cancellationToken);
            
            if (!isFloorExist) return Result.Failure(FloorErrors.NotFound(command.FloorDto.FloorId));
            
            if (!await floorUniqueness.IsUniqueAsync(
                    command.FloorDto.Number, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure(FloorErrors.FloorAlreadyExistOrWrong);
            
            var existingFloor = await repository.GetByIdAsync(command.FloorDto.FloorId, cancellationToken: cancellationToken);
            
            repository.Update(existingFloor);

            return Result.Success();
        }
    }
}