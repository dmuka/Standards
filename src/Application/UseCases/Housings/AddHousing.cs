using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class AddHousing
{
    public class Command(HousingDto2 housing) : IRequest<Result<Housing>>
    {
        public HousingDto2 HousingDto { get; set; } = housing;
    };

    public class CommandHandler(IHousingRepository repository) : IRequestHandler<Command, Result<Housing>>
    {
        public async Task<Result<Housing>> Handle(Command command, CancellationToken cancellationToken)
        {
            var housingCreationResult = Housing.Create(
                command.HousingDto.HousingName, 
                command.HousingDto.HousingShortName, 
                command.HousingDto.Address,
                command.HousingDto.HousingId,
                command.HousingDto.Comments);

            if (housingCreationResult.IsFailure) return Result.Failure<Housing>(housingCreationResult.Error);

            var housing = housingCreationResult.Value;
            
            await repository.AddAsync(housing, cancellationToken);
            
            return Result.Success(housing);
        }
    }
}