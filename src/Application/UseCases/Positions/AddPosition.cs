using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Positions;
using MediatR;

namespace Application.UseCases.Positions;

public class AddPosition
{
    public class Command(PositionDto2 position) : IRequest<Result<Position>>
    {
        public PositionDto2 PositionDto { get; set; } = position;
    };

    public class CommandHandler(
        IPositionRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Position>>
    {
        public async Task<Result<Position>> Handle(Command command, CancellationToken cancellationToken)
        {
            var positionCreationResult = Position.Create(
                command.PositionDto.PositionName, 
                command.PositionDto.PositionShortName,
                command.PositionDto.Id,
                command.PositionDto.Comments);

            if (positionCreationResult.IsFailure) return Result.Failure<Position>(positionCreationResult.Error);

            var position = positionCreationResult.Value;
            
            await repository.AddAsync(position, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(position);
        }
    }
}