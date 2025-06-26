using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class EditHousing
{
    public class Command : IRequest<Result<int>>
    {
        public required HousingDto2 HousingDto { get; set; }
    }

    public class CommandHandler(ApplicationDbContext dbContext) 
        : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var existingHousing = await dbContext.Housings2.FindAsync(
                [command.HousingDto.HousingId], 
                cancellationToken: cancellationToken);
            if (existingHousing is null) return Result.Failure<int>(HousingErrors.NotFound(command.HousingDto.HousingId));
            
            var housingCreationResult = Housing.Create(
                command.HousingDto.HousingName,
                command.HousingDto.HousingShortName,
                command.HousingDto.Address,
                command.HousingDto.HousingId,
                command.HousingDto.Comments);

            if (housingCreationResult.IsFailure) return Result.Failure<int>(housingCreationResult.Error);
            
            dbContext.Update(housingCreationResult.Value);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return number;
        }
    }
}