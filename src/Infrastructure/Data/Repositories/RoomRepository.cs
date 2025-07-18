using Domain.Aggregates.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Rooms2
            .AsNoTracking()
            .FirstOrDefaultAsync(room => room.Id == id, cancellationToken);
    }

    public async Task<Room[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Rooms2
            .AsNoTracking()
            .Where(room => ids.Contains(room.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Room[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Rooms2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Room room, CancellationToken cancellationToken)
    {
        await context.Rooms2.AddAsync(room, cancellationToken);
    }

    public void Remove(Room room)
    {
        context.Rooms2.Remove(room);
    }

    public void Update(Room room)
    {
        context.Rooms2.Update(room);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Rooms2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }
}