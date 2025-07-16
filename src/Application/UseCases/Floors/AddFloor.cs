using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core;
using Domain.Aggregates.Floors;
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
        IFloorUniqueness floorUniqueness,
        IUnitOfWork unitOfWork,
        ILogger<AddFloor> logger) : IRequestHandler<Command, Result<Floor>>
    {
        public async Task<Result<Floor>> Handle(Command command, CancellationToken cancellationToken)
        {
            if (!await floorUniqueness.IsUniqueAsync(
                    command.FloorDto.Number, 
                    command.FloorDto.HousingId, 
                    cancellationToken))
                return Result.Failure<Floor>(FloorErrors.FloorAlreadyExistOrWrong);
            
            var floorCreationResult = Floor.Create(command.FloorDto.Number, command.FloorDto.HousingId);

            if (floorCreationResult.IsFailure) return Result.Failure<Floor>(floorCreationResult.Error);

            await unitOfWork.BeginTransactionAsync(cancellationToken);

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