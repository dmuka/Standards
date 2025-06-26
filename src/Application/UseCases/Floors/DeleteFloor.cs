using Core;
using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Floors;

public class DeleteFloor
{
    public class Command : IRequest<Result<int>>
    {
        public required FloorId FloorId { get; set; }
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