using Core;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class DeleteHousing
{
    public class Command(HousingId housingId) : IRequest<Result<int>>
    {
        public HousingId HousingId { get; set; } = housingId;
    }
    
    public class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingHousing = await dbContext.Housings2.FindAsync(
                [command.HousingId], 
                cancellationToken: cancellationToken);
            if (existingHousing is null) return Result.Failure<int>(HousingErrors.NotFound(command.HousingId));
            
            dbContext.Housings2.Remove(existingHousing);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success(number);
        }
    }
}