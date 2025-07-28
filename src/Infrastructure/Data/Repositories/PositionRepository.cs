using System.Linq.Expressions;
using Domain.Aggregates.Positions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class PositionRepository(ApplicationDbContext context) : IPositionRepository
{
    public async Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Positions2
            .AsNoTracking()
            .FirstOrDefaultAsync(position => position.Id == id, cancellationToken);
    }

    public async Task<Position[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Positions2
            .AsNoTracking()
            .Where(position => ids.Contains(position.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Position[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Positions2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Position position, CancellationToken cancellationToken)
    {
        await context.Positions2.AddAsync(position, cancellationToken);
    }

    public void Remove(Position position)
    {
        context.Positions2.Remove(position);
    }

    public void Update(Position position)
    {
        context.Positions2.Update(position);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Positions2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }

    public IQueryable<Position> Where(Expression<Func<Position, bool>> func)
    {
        return context.Positions2
            .AsNoTracking()
            .Where(func);
    }
}