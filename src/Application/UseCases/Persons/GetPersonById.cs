using Core.Results;
using Domain.Aggregates.Persons;
using MediatR;

namespace Application.UseCases.Persons;

public class GetPersonById
{
    public class Query(PersonId personId) : IRequest<Result<Person>>
    {
        public PersonId PersonId { get; } = personId;
    }
    
    public class QueryHandler(IPersonRepository repository) : IRequestHandler<Query, Result<Person>>
    {
        public async Task<Result<Person>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isPersonExist = await repository.ExistsAsync(query.PersonId, cancellationToken);
            
            if (!isPersonExist) return Result.Failure<Person>(PersonErrors.NotFound(query.PersonId));
            
            var person = await repository.GetByIdAsync(query.PersonId, cancellationToken);

            return person;
        }
    }
}