using Domain.Aggregates.Floors;
using MediatR;

namespace Application.UseCases.Floors;

public class GetAllFloors
{
    public class Query : IRequest<IList<Floor>>;
    
    public class QueryHandler(IFloorRepository repository) : IRequestHandler<Query, IList<Floor>>
    {
        public async Task<IList<Floor>> Handle(Query request, CancellationToken cancellationToken)
        {
            var floors = await repository.GetAllAsync(cancellationToken);

            return floors;
        }
    }
}