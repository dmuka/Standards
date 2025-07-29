using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Workplaces;
using MediatR;

namespace Application.UseCases.Workplaces;

public class DeleteWorkplace
{
    public class Command(WorkplaceId workplaceId) : IRequest<Result>
    {
        public WorkplaceId WorkplaceId { get; } = workplaceId;
    }
    
    public class CommandHandler(
        IWorkplaceRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isWorkplaceExist = await repository.ExistsAsync(command.WorkplaceId, cancellationToken);
            
            if (!isWorkplaceExist) return Result.Failure<int>(WorkplaceErrors.NotFound(command.WorkplaceId));
            
            var existingWorkplace = await repository.GetByIdAsync(command.WorkplaceId, cancellationToken: cancellationToken);
            
            repository.Remove(existingWorkplace!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}