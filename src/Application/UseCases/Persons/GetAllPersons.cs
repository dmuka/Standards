using Domain.Aggregates.Persons;
using MediatR;

namespace Application.UseCases.Persons;

public class GetAllPersons
{
    public class Query : IRequest<IList<Person>>;
    
    public class QueryHandler(IPersonRepository repository) : IRequestHandler<Query, IList<Person>>
    {
        public async Task<IList<Person>> Handle(Query request, CancellationToken cancellationToken)
        {
            var persons = await repository.GetAllAsync(cancellationToken);

            return persons;
        }
    }
}