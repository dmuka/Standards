using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Floors;

public class GetAllFloors
{
    public class Query : IRequest<IList<Floor>>;
    
    public class QueryHandler(ApplicationDbContext dbContext) : IRequestHandler<Query, IList<Floor>>
    {
        public async Task<IList<Floor>> Handle(Query request, CancellationToken cancellationToken)
        {
            var floors = await dbContext.Floors
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return floors;
        }
    }
}