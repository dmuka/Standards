using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class GetAllHousings
{
    public class Query : IRequest<IList<Housing>>;
    
    public class QueryHandler(IHousingRepository repository) : IRequestHandler<Query, IList<Housing>>
    {
        public async Task<IList<Housing>> Handle(Query request, CancellationToken cancellationToken)
        {
            var housings = await repository.GetAllAsync(cancellationToken);

            return housings;
        }
    }
}