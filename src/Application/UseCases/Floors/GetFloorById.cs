using Domain.Aggregates.Floors;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Floors;

public class GetFloorById
{
    public class Query : IRequest<Floor?>
    {
        public required FloorId FloorId { get; set; }
    }
    
    public class QueryHandler(ApplicationDbContext dbContext) : IRequestHandler<Query, Floor?>
    {
        public async Task<Floor?> Handle(Query query, CancellationToken cancellationToken)
        {
            var floor = await dbContext.Floors
                .AsNoTracking()
                .FirstOrDefaultAsync(housing => housing.Id == query.FloorId, cancellationToken);

            return floor;
        }
    }
}