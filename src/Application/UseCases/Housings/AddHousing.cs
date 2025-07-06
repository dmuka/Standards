using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class AddHousing
{
    public class Command(HousingDto2 housing) : IRequest<Result<int>>
    {
        public HousingDto2 HousingDto { get; set; } = housing;
    };

    public class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken)
        {
            var housingCreationResult = Housing.Create(
                command.HousingDto.HousingName, 
                command.HousingDto.HousingShortName, 
                command.HousingDto.Address,
                command.HousingDto.HousingId,
                command.HousingDto.Comments);

            if (housingCreationResult.IsFailure) return Result.Failure<int>(housingCreationResult.Error);
            
            await dbContext.Housings2.AddAsync(housingCreationResult.Value, cancellationToken);
            var number = await dbContext.SaveChangesAsync(cancellationToken);
            
            return Result.Success(number);
        }
    }
}