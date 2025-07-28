using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Positions;
using MediatR;

namespace Application.UseCases.Positions;

public class DeletePosition
{
    public class Command(PositionId positionId) : IRequest<Result>
    {
        public PositionId PositionId { get; set; } = positionId;
    }
    
    public class CommandHandler(
        IPositionRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isPositionExist = await repository.ExistsAsync(command.PositionId, cancellationToken);
            
            if (!isPositionExist) return Result.Failure<int>(PositionErrors.NotFound(command.PositionId));
            
            var existingPosition = await repository.GetByIdAsync(command.PositionId, cancellationToken: cancellationToken);
            
            repository.Remove(existingPosition!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}