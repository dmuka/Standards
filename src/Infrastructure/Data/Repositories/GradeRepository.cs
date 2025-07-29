using System.Linq.Expressions;
using Domain.Aggregates.Grades;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GradeRepository(ApplicationDbContext context) : IGradeRepository
{
    public async Task<Grade?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Grades2
            .AsNoTracking()
            .FirstOrDefaultAsync(grade => grade.Id == id, cancellationToken);
    }

    public async Task<Grade[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Grades2
            .AsNoTracking()
            .Where(grade => ids.Contains(grade.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Grade[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Grades2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Grade grade, CancellationToken cancellationToken)
    {
        await context.Grades2.AddAsync(grade, cancellationToken);
    }

    public void Remove(Grade grade)
    {
        context.Grades2.Remove(grade);
    }

    public void Update(Grade grade)
    {
        context.Grades2.Update(grade);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Grades2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }

    public IQueryable<Grade> Where(Expression<Func<Grade, bool>> func)
    {
        return context.Grades2
            .AsNoTracking()
            .Where(func);
    }
}