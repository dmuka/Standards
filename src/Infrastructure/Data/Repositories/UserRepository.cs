using Domain.Aggregates.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users2
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<User[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Users2
            .AsNoTracking()
            .Where(user => ids.Contains(user.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<User[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Users2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users2.AddAsync(user, cancellationToken);
    }

    public void Remove(User user)
    {
        context.Users2.Remove(user);
    }

    public void Update(User user)
    {
        context.Users2.Update(user);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users2
            .AsNoTracking()
            .AnyAsync(user => user.Id == id, cancellationToken);
    }
}