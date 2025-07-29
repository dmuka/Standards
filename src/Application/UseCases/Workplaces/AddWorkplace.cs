using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Workplaces;
using MediatR;

namespace Application.UseCases.Workplaces;

public class AddWorkplace
{
    public class Command(WorkplaceDto2 workplace) : IRequest<Result<Workplace>>
    {
        public WorkplaceDto2 WorkplaceDto { get; } = workplace;
    };

    public class CommandHandler(
        IWorkplaceRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Workplace>>
    {
        public async Task<Result<Workplace>> Handle(Command command, CancellationToken cancellationToken)
        {
            var workplaceCreationResult = Workplace.Create(
                command.WorkplaceDto.WorkplaceName, 
                command.WorkplaceDto.WorkplaceShortName, 
                command.WorkplaceDto.RoomId, 
                command.WorkplaceDto.ResponsibleId, 
                command.WorkplaceDto.SectorId, 
                command.WorkplaceDto.Id, 
                command.WorkplaceDto.ImagePath,
                command.WorkplaceDto.Comments);

            if (workplaceCreationResult.IsFailure) return Result.Failure<Workplace>(workplaceCreationResult.Error);

            var workplace = workplaceCreationResult.Value;
            
            await repository.AddAsync(workplace, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(workplace);
        }
    }
}