using Domain.Aggregates.Workplaces;
using MediatR;

namespace Application.UseCases.Workplaces;

public class GetAllWorkplaces
{
    public class Query : IRequest<IList<Workplace>>;
    
    public class QueryHandler(IWorkplaceRepository repository) : IRequestHandler<Query, IList<Workplace>>
    {
        public async Task<IList<Workplace>> Handle(Query request, CancellationToken cancellationToken)
        {
            var workplaces = await repository.GetAllAsync(cancellationToken);

            return workplaces;
        }
    }
}