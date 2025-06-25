using Application.UseCases.DTOs;
using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Floors;

public class CreateFloor
{
    public class Command() : IRequest<int>
    {
        public FloorDto FloorDto { get; set; }
    };

    public class CommandHandler(ApplicationDbContext dbContext, IFloorUniqueness floorUniqueness) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command command, CancellationToken cancellationToken)
        {
            var floorCreationResult = await Floor.Create(
                command.FloorDto.Number, 
                command.FloorDto.HousingId, 
                floorUniqueness,
                cancellationToken);

            if (floorCreationResult.IsFailure) return 0;
            
            await dbContext.Floors.AddAsync(floorCreationResult.Value, cancellationToken);
            var number = await dbContext.SaveChangesAsync(cancellationToken);
            
            return number;
        }
    }
}