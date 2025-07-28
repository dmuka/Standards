using Core.Results;
using Domain.Aggregates.Positions;
using MediatR;

namespace Application.UseCases.Positions;

public class GetPositionById
{
    public class Query(PositionId positionId) : IRequest<Result<Position>>
    {
        public PositionId PositionId { get; set; } = positionId;
    }
    
    public class QueryHandler(IPositionRepository repository) : IRequestHandler<Query, Result<Position>>
    {
        public async Task<Result<Position>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isPositionExist = await repository.ExistsAsync(query.PositionId, cancellationToken);
            
            if (!isPositionExist) return Result.Failure<Position>(PositionErrors.NotFound(query.PositionId));
            
            var position = await repository.GetByIdAsync(query.PositionId, cancellationToken);

            return position;
        }
    }
}