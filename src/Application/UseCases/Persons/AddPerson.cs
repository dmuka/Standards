using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Persons;
using MediatR;

namespace Application.UseCases.Persons;

public class AddPerson
{
    public class Command(PersonDto2 person) : IRequest<Result<Person>>
    {
        public PersonDto2 PersonDto { get; } = person;
    };

    public class CommandHandler(
        IPersonRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Person>>
    {
        public async Task<Result<Person>> Handle(Command command, CancellationToken cancellationToken)
        {
            var personCreationResult = Person.Create(
                command.PersonDto.FirstName, 
                command.PersonDto.MiddleName, 
                command.PersonDto.LastName, 
                command.PersonDto.BirthdayDate, 
                command.PersonDto.UserId,
                command.PersonDto.Id,
                command.PersonDto.Comments);

            if (personCreationResult.IsFailure) return Result.Failure<Person>(personCreationResult.Error);

            var person = personCreationResult.Value;
            
            await repository.AddAsync(person, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(person);
        }
    }
}