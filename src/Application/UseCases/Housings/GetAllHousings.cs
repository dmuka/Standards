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
                .Include(housing => housing.FloorIds)
                .ToListAsync(cancellationToken);

            return housings;
        }
    }
}