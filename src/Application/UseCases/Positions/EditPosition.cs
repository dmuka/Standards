using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Positions;
using MediatR;

namespace Application.UseCases.Positions;

public class EditPosition
{
    public class Command(PositionDto2 position) : IRequest<Result>
    {
        public PositionDto2 PositionDto { get; set; } = position;
    }

    public class CommandHandler(
        IPositionRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isPositionExist = await repository.ExistsAsync(command.PositionDto.Id, cancellationToken);
            
            if (!isPositionExist) return Result.Failure(PositionErrors.NotFound(command.PositionDto.Id));
            
            var existingPosition = await repository.GetByIdAsync(command.PositionDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingPosition!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}