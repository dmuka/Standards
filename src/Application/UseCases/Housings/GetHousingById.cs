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
                .Include(housing => housing.FloorIds)
                .FirstOrDefaultAsync(housing => housing.Id == query.HousingId, cancellationToken);

            return housing;
        }
    }
}