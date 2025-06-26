using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Floors;

public class AddFloor
{
    public class Command(FloorDto floor) : IRequest<Result<int>>
    {
        public FloorDto FloorDto { get; set; } = floor;
    };

    public class CommandHandler(ApplicationDbContext dbContext, IFloorUniqueness floorUniqueness) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await floorUniqueness.IsUniqueAsync(
                    command.FloorDto.Number, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure<int>(FloorErrors.FloorAlreadyExist);
            
            var floorCreationResult = Floor.Create(command.FloorDto.Number, command.FloorDto.HousingId);

            if (floorCreationResult.IsFailure) return 0;
            
            await dbContext.Floors.AddAsync(floorCreationResult.Value, cancellationToken);
            var number = await dbContext.SaveChangesAsync(cancellationToken);
            
            return number;
        }
    }
}