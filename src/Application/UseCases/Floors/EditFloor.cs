using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Floors;

public class EditFloor
{
    public class Command(FloorDto floor) : IRequest<Result<int>>
    {
        public FloorDto FloorDto { get; set; } = floor;
    }
    
    public class CommandHandler(ApplicationDbContext dbContext, IFloorUniqueness floorUniqueness) 
        : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingFloor = await dbContext.Floors.FindAsync(
                [command.FloorDto.FloorId], 
                cancellationToken: cancellationToken);
            if (existingFloor is null) return Result.Failure<int>(FloorErrors.NotFound(command.FloorDto.FloorId));
            
            if (!await floorUniqueness.IsUniqueAsync(
                    command.FloorDto.Number, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure<int>(FloorErrors.FloorAlreadyExistOrWrong);

            dbContext.Update(existingFloor);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return number;
        }
    }
}