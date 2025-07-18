using Core.Results;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using MediatR;

namespace Application.UseCases.Floors;


public class GetFloorsByHousingId
{
    public class Query(HousingId[] housingIds) : IRequest<Result<IList<Floor>>>
    {
        public HousingId[] HousingIds { get; set; } = housingIds;
    }

    public class QueryHandler(
        IHousingRepository housingRepository, 
        IFloorRepository floorRepository) : IRequestHandler<Query, Result<IList<Floor>>>
    {
        public async Task<Result<IList<Floor>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var housings = await housingRepository.GetByIdsAsync(
                request.HousingIds.Select(id => id.Value).ToArray(), 
                cancellationToken);

            var floorsIds = housings.SelectMany(housing => housing.FloorIds.Select(id => id.Value)).ToArray();

            var floors = await floorRepository.GetByIdsAsync(floorsIds, cancellationToken);

            return floors;
        }
    }
}
