using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class GetAllSectors
{
    public class Query : IRequest<IList<Sector>>;
    
    public class QueryHandler(ISectorRepository repository) : IRequestHandler<Query, IList<Sector>>
    {
        public async Task<IList<Sector>> Handle(Query request, CancellationToken cancellationToken)
        {
            var sectors = await repository.GetAllAsync(cancellationToken);

            return sectors;
        }
    }
}