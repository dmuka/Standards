using Core.Results;
using Domain.Aggregates.Workplaces;
using MediatR;

namespace Application.UseCases.Workplaces;

public class GetWorkplaceById
{
    public class Query(WorkplaceId workplaceId) : IRequest<Result<Workplace>>
    {
        public WorkplaceId WorkplaceId { get; } = workplaceId;
    }
    
    public class QueryHandler(IWorkplaceRepository repository) : IRequestHandler<Query, Result<Workplace>>
    {
        public async Task<Result<Workplace>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isWorkplaceExist = await repository.ExistsAsync(query.WorkplaceId, cancellationToken);
            
            if (!isWorkplaceExist) return Result.Failure<Workplace>(WorkplaceErrors.NotFound(query.WorkplaceId));
            
            var workplace = await repository.GetByIdAsync(query.WorkplaceId, cancellationToken);

            return workplace;
        }
    }
}