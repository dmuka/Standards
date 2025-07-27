using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class AddSector
{
    public class Command(SectorDto2 sector) : IRequest<Result<Sector>>
    {
        public SectorDto2 SectorDto { get; set; } = sector;
    };

    public class CommandHandler(
        ISectorRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Sector>>
    {
        public async Task<Result<Sector>> Handle(Command command, CancellationToken cancellationToken)
        {
            var sectorCreationResult = Sector.Create(
                command.SectorDto.SectorName, 
                command.SectorDto.SectorShortName,
                command.SectorDto.Id,
                command.SectorDto.DepartmentId,
                command.SectorDto.Comments);

            if (sectorCreationResult.IsFailure) return Result.Failure<Sector>(sectorCreationResult.Error);

            var sector = sectorCreationResult.Value;
            
            await repository.AddAsync(sector, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(sector);
        }
    }
}