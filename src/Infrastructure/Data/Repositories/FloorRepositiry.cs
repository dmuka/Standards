using Domain.Aggregates.Floors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class FloorRepository(ApplicationDbContext context) : IFloorRepository
{
    public async Task<Floor?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Floors
            .AsNoTracking()
            .FirstOrDefaultAsync(floor => floor.Id == id, cancellationToken);
    }
    
    public async Task<Floor[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Floors
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Floor floor, CancellationToken cancellationToken)
    {
        await context.Floors.AddAsync(floor, cancellationToken);
    }

    public void Remove(Floor floor)
    {
        context.Floors.Remove(floor);
    }

    public void Update(Floor floor)
    {
        context.Floors.Update(floor);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Floors.AnyAsync(floor => floor.Id == id, cancellationToken);
    }
}