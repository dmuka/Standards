using System.Linq.Expressions;
using Domain.Aggregates.Categories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Categories2
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);
    }

    public async Task<Category[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Categories2
            .AsNoTracking()
            .Where(category => ids.Contains(category.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Category[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Categories2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await context.Categories2.AddAsync(category, cancellationToken);
    }

    public void Remove(Category category)
    {
        context.Categories2.Remove(category);
    }

    public void Update(Category category)
    {
        context.Categories2.Update(category);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Categories2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }

    public IQueryable<Category> Where(Expression<Func<Category, bool>> func)
    {
        return context.Categories2
            .AsNoTracking()
            .Where(func);
    }
}