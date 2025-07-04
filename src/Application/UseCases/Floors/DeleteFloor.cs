using Core;
using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Floors;

public class DeleteFloor
{
    public class Command(FloorId floorId) : IRequest<Result<int>>
    {
        public FloorId FloorId { get; set; } = floorId;
    }
    
    public class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingFloor = await dbContext.Floors.FindAsync(
                [command.FloorId], 
                cancellationToken: cancellationToken);
            if (existingFloor is null) return Result.Failure<int>(FloorErrors.NotFound(command.FloorId));
            
            dbContext.Floors.Remove(existingFloor);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(number);
        }
    }
}