using Domain.Aggregates.Housings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class HousingRepository(ApplicationDbContext context) : IHousingRepository
{
    public async Task<Housing?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Housings2
            .AsNoTracking()
            .FirstOrDefaultAsync(housing => housing.Id == id, cancellationToken);
    }
    
    public async Task<Housing[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Housings2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Housing housing, CancellationToken cancellationToken)
    {
        await context.Housings2.AddAsync(housing, cancellationToken);
    }

    public void Remove(Housing housing)
    {
        context.Housings2.Remove(housing);
    }

    public void Update(Housing housing)
    {
        context.Housings2.Update(housing);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Housings2.AnyAsync(housing => housing.Id == id, cancellationToken);
    }
}