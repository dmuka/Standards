using Domain.Aggregates.Sectors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class SectorRepository(ApplicationDbContext context) : ISectorRepository
{
    public async Task<Sector?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .FirstOrDefaultAsync(room => room.Id == id, cancellationToken);
    }

    public async Task<Sector[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .Where(room => ids.Contains(room.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Sector[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Sector room, CancellationToken cancellationToken)
    {
        await context.Sectors2.AddAsync(room, cancellationToken);
    }

    public void Remove(Sector room)
    {
        context.Sectors2.Remove(room);
    }

    public void Update(Sector room)
    {
        context.Sectors2.Update(room);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }
}