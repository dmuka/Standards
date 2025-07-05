using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Housings;

public class GetAllHousings
{
    public class Query : IRequest<IList<Housing>>;
    
    public class QueryHandler(ApplicationDbContext dbContext) : IRequestHandler<Query, IList<Housing>>
    {
        public async Task<IList<Housing>> Handle(Query request, CancellationToken cancellationToken)
        {
            var housings = await dbContext.Housings2
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var housing in housings)
            {
                var floorIds = await dbContext.Floors
                    .Where(floor => floor.HousingId == (HousingId)housing.Id)
                    .Select(floor => (FloorId)floor.Id)
                    .ToListAsync(cancellationToken);

                housing.AddFloors(floorIds);
            }

            return housings;
        }
    }
}