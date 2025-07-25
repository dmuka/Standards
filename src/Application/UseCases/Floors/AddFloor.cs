using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Floors;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Floors;

public class AddFloor
{
    public class Command(FloorDto floor) : IRequest<Result<Floor>>
    {
        public FloorDto FloorDto { get; set; } = floor;
    }

    public class CommandHandler(
        IFloorRepository repository, 
        IChildEntityUniqueness childEntityUniqueness,
        IUnitOfWork unitOfWork,
        ILogger<AddFloor> logger) : IRequestHandler<Command, Result<Floor>>
    {
        public async Task<Result<Floor>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await childEntityUniqueness.IsUniqueAsync<FloorDto, HousingDto2>(
                    command.FloorDto.Id, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure<Floor>(FloorErrors.FloorAlreadyExistOrWrong);
            
            var floorCreationResult = Floor.Create(command.FloorDto.Number, command.FloorDto.HousingId);

            if (floorCreationResult.IsFailure) return Result.Failure<Floor>(floorCreationResult.Error);

            var floor = floorCreationResult.Value;
            
            try
            {
                await repository.AddAsync(floor, cancellationToken);
                await unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync(cancellationToken);
                logger.LogError("Rollback changes: handler - {Handler}, floor id - {Id}, message - {Message}", 
                    nameof(AddFloor), floor.Id, ex.Message);
                
                throw;
            }
            
            return Result.Success(floor);
        }
    }
}