using Application.UseCases.DTOs;
using Infrastructure.Data;
using MediatR;

namespace Application.UseCases.Housings;

public class AddHousing
{
    public class Command(HousingDto2 housing) : IRequest<int>
    {
        public HousingDto2 HousingDto { get; set; } = housing;
    };

    public class CommandHandler(ApplicationDbContext dbContext) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command command, CancellationToken cancellationToken)
        {
            var housingCreationResult = Domain.Aggregates.Housings.Housing.Create(
                command.HousingDto.HousingName, 
                command.HousingDto.HousingShortName, 
                command.HousingDto.Address,
                command.HousingDto.HousingId,
                command.HousingDto.Comments);

            if (housingCreationResult.IsFailure) return 0;
            
            await dbContext.Housings2.AddAsync(housingCreationResult.Value, cancellationToken);
            var number = await dbContext.SaveChangesAsync(cancellationToken);
            
            return number;
        }
    }
}