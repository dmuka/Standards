using Core.Results;
using Domain.Aggregates.Floors;
using MediatR;

namespace Application.UseCases.Floors;

public class GetFloorById
{
    public class Query(FloorId floorId) : IRequest<Result<Floor>>
    {
        public FloorId FloorId { get; set; } = floorId;
    }
    
    public class QueryHandler(IFloorRepository repository) : IRequestHandler<Query, Result<Floor>>
    {
        public async Task<Result<Floor>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isFloorExist = await repository.ExistsAsync(query.FloorId, cancellationToken);
            
            if (!isFloorExist) return Result.Failure<Floor>(FloorErrors.NotFound(query.FloorId));
            
            var floor = await repository.GetByIdAsync(query.FloorId, cancellationToken);

            return floor ;
        }
    }
}