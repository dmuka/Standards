using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Persons;
using MediatR;

namespace Application.UseCases.Persons;

public class EditPerson
{
    public class Command(PersonDto2 person) : IRequest<Result>
    {
        public PersonDto2 PersonDto { get; } = person;
    }

    public class CommandHandler(
        IPersonRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isPersonExist = await repository.ExistsAsync(command.PersonDto.Id, cancellationToken);
            
            if (!isPersonExist) return Result.Failure(PersonErrors.NotFound(command.PersonDto.Id));
            
            var existingPerson = await repository.GetByIdAsync(command.PersonDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingPerson!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}