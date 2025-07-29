using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Workplaces;
using MediatR;

namespace Application.UseCases.Workplaces;

public class EditWorkplace
{
    public class Command(WorkplaceDto2 workplace) : IRequest<Result>
    {
        public WorkplaceDto2 WorkplaceDto { get; } = workplace;
    }

    public class CommandHandler(
        IWorkplaceRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isWorkplaceExist = await repository.ExistsAsync(command.WorkplaceDto.Id, cancellationToken);
            
            if (!isWorkplaceExist) return Result.Failure(WorkplaceErrors.NotFound(command.WorkplaceDto.Id));
            
            var existingWorkplace = await repository.GetByIdAsync(command.WorkplaceDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingWorkplace!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}