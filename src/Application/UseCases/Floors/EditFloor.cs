using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Floors;
using Domain.Services;
using MediatR;

namespace Application.UseCases.Floors;

public class EditFloor
{
    public class Command(FloorDto floor) : IRequest<Result>
    {
        public FloorDto FloorDto { get; set; } = floor;
    }
    
    public class CommandHandler(IFloorRepository repository, IChildEntityUniqueness childEntityUniqueness) 
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isFloorExist = await repository.ExistsAsync(command.FloorDto.Id, cancellationToken);
            
            if (!isFloorExist) return Result.Failure(FloorErrors.NotFound(command.FloorDto.Id));
            
            if (!await childEntityUniqueness.IsUniqueAsync<FloorDto, HousingDto2>(
                    command.FloorDto.Id, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure(FloorErrors.FloorAlreadyExistOrWrong);
            
            var existingFloor = await repository.GetByIdAsync(command.FloorDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingFloor!);

            return Result.Success();
        }
    }
}