using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class EditHousing
{
    public class Command(HousingDto2 housing) : IRequest<Result<int>>
    {
        public HousingDto2 HousingDto { get; set; } = housing;
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
            
            var housingUpdateResult = existingHousing.Update(
                command.HousingDto.HousingName,
                command.HousingDto.HousingShortName,
                command.HousingDto.Address,
                command.HousingDto.Comments);

            if (housingUpdateResult.IsFailure) return Result.Failure<int>(housingUpdateResult.Error);
            
            dbContext.Update(existingHousing);
            var number = await dbContext.SaveChangesAsync(cancellationToken);

            return number;
        }
    }
}