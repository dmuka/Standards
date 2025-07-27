using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class DeleteSector
{
    public class Command(SectorId housingId) : IRequest<Result>
    {
        public SectorId SectorId { get; set; } = housingId;
    }
    
    public class CommandHandler(
        ISectorRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isSectorExist = await repository.ExistsAsync(command.SectorId, cancellationToken);
            
            if (!isSectorExist) return Result.Failure<int>(SectorErrors.NotFound(command.SectorId));
            
            var existingSector = await repository.GetByIdAsync(command.SectorId, cancellationToken: cancellationToken);
            
            repository.Remove(existingSector!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}