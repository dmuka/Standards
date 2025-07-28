using Domain.Aggregates.Positions;
using MediatR;

namespace Application.UseCases.Positions;

public class GetAllPositions
{
    public class Query : IRequest<IList<Position>>;
    
    public class QueryHandler(IPositionRepository repository) : IRequestHandler<Query, IList<Position>>
    {
        public async Task<IList<Position>> Handle(Query request, CancellationToken cancellationToken)
        {
            var positions = await repository.GetAllAsync(cancellationToken);

            return positions;
        }
    }
}