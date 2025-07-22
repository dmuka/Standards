using Domain.Aggregates.Sectors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class SectorRepository(ApplicationDbContext context) : ISectorRepository
{
    public async Task<Sector?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .FirstOrDefaultAsync(sector => sector.Id == id, cancellationToken);
    }

    public async Task<Sector[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .Where(sector => ids.Contains(sector.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Sector[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Sector sector, CancellationToken cancellationToken)
    {
        await context.Sectors2.AddAsync(sector, cancellationToken);
    }

    public void Remove(Sector sector)
    {
        context.Sectors2.Remove(sector);
    }

    public void Update(Sector sector)
    {
        context.Sectors2.Update(sector);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Sectors2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }
}