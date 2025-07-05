using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Housings;

public class GetHousingById
{
    public class Query(HousingId housingId) : IRequest<Housing?>
    {
        public HousingId HousingId { get; set; } = housingId;
    }
    
    public class QueryHandler(ApplicationDbContext dbContext) : IRequestHandler<Query, Housing?>
    {
        public async Task<Housing?> Handle(Query query, CancellationToken cancellationToken)
        {
            var housing = await dbContext.Housings2
                .AsNoTracking()
                .FirstOrDefaultAsync(housing => housing.Id == query.HousingId, cancellationToken);

            if (housing is null) return housing;
            
            var floorIds = dbContext.Floors
                .Where(floor => floor.HousingId == housing.Id)
                .Select(floor => (FloorId)floor.Id)
                .ToList();

            housing.AddFloors(floorIds);

            return housing;
        }
    }
}