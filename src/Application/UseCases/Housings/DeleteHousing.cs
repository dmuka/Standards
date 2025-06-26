using Core;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class DeleteHousing
{
    public class Command : IRequest<Result<int>>
    {
        public required HousingId HousingId { get; set; }
    }
    
    public class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingHousing = await dbContext.Housings.FindAsync(
                [command.HousingId], 
                cancellationToken: cancellationToken);
            if (existingHousing is null) return Result.Failure<int>(HousingErrors.NotFound(command.HousingId));
            
            dbContext.Housings.Remove(existingHousing);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(number);
        }
    }
}