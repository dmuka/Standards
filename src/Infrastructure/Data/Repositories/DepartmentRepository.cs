using System.Linq.Expressions;
using Domain.Aggregates.Departments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
{
    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Departments2
            .AsNoTracking()
            .FirstOrDefaultAsync(department => department.Id == id, cancellationToken);
    }

    public async Task<Department[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Departments2
            .AsNoTracking()
            .Where(department => ids.Contains(department.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Department[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Departments2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Department department, CancellationToken cancellationToken)
    {
        await context.Departments2.AddAsync(department, cancellationToken);
    }

    public void Remove(Department department)
    {
        context.Departments2.Remove(department);
    }

    public void Update(Department department)
    {
        context.Departments2.Update(department);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Departments2
            .AsNoTracking()
            .AnyAsync(room => room.Id == id, cancellationToken);
    }

    public IQueryable<Department> Where(Expression<Func<Department, bool>> func)
    {
        return context.Departments2
            .AsNoTracking()
            .Where(func);
    }
}