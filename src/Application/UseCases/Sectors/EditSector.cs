using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class EditSector
{
    public class Command(SectorDto2 housing) : IRequest<Result>
    {
        public SectorDto2 SectorDto { get; set; } = housing;
    }

    public class CommandHandler(
        ISectorRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isSectorExist = await repository.ExistsAsync(command.SectorDto.Id, cancellationToken);
            
            if (!isSectorExist) return Result.Failure(SectorErrors.NotFound(command.SectorDto.Id));
            
            var existingSector = await repository.GetByIdAsync(command.SectorDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingSector!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}