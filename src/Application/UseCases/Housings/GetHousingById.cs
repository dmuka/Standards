using Core.Results;
using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Housings;

public class GetHousingById
{
    public class Query(HousingId housingId) : IRequest<Result<Housing>>
    {
        public HousingId HousingId { get; set; } = housingId;
    }
    
    public class QueryHandler(IHousingRepository repository) : IRequestHandler<Query, Result<Housing>>
    {
        public async Task<Result<Housing>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isHousingExist = await repository.ExistsAsync(query.HousingId, cancellationToken);
            
            if (!isHousingExist) return Result.Failure<Housing>(HousingErrors.NotFound(query.HousingId));
            
            var housing = await repository.GetByIdAsync(query.HousingId, cancellationToken);

            return housing;
        }
    }
}