using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Persons;
using MediatR;

namespace Application.UseCases.Persons;

public class DeletePerson
{
    public class Command(PersonId personId) : IRequest<Result>
    {
        public PersonId PersonId { get; } = personId;
    }
    
    public class CommandHandler(
        IPersonRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isPersonExist = await repository.ExistsAsync(command.PersonId, cancellationToken);
            
            if (!isPersonExist) return Result.Failure<int>(PersonErrors.NotFound(command.PersonId));
            
            var existingPerson = await repository.GetByIdAsync(command.PersonId, cancellationToken: cancellationToken);
            
            repository.Remove(existingPerson!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}