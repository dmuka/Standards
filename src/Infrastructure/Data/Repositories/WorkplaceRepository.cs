using System.Linq.Expressions;
using Domain.Aggregates.Workplaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class WorkplaceRepository(ApplicationDbContext context) : IWorkplaceRepository
{
    public async Task<Workplace?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Workplaces2
            .AsNoTracking()
            .FirstOrDefaultAsync(workplace => workplace.Id == id, cancellationToken);
    }

    public async Task<Workplace[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Workplaces2
            .AsNoTracking()
            .Where(workplace => ids.Contains(workplace.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Workplace[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Workplaces2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Workplace workplace, CancellationToken cancellationToken)
    {
        await context.Workplaces2.AddAsync(workplace, cancellationToken);
    }

    public void Remove(Workplace workplace)
    {
        context.Workplaces2.Remove(workplace);
    }

    public void Update(Workplace workplace)
    {
        context.Workplaces2.Update(workplace);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Workplaces2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }

    public IQueryable<Workplace> Where(Expression<Func<Workplace, bool>> func)
    {
        return context.Workplaces2
            .AsNoTracking()
            .Where(func);
    }
}