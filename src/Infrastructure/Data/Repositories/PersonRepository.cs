using Domain.Aggregates.Persons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class PersonRepository(ApplicationDbContext context) : IPersonRepository
{
    public async Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Persons2
            .AsNoTracking()
            .FirstOrDefaultAsync(person => person.Id == id, cancellationToken);
    }

    public async Task<Person[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
    {
        return await context.Persons2
            .AsNoTracking()
            .Where(person => ids.Contains(person.Id))
            .ToArrayAsync(cancellationToken);
    }
    
    public async Task<Person[]> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Persons2
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(Person person, CancellationToken cancellationToken)
    {
        await context.Persons2.AddAsync(person, cancellationToken);
    }

    public void Remove(Person person)
    {
        context.Persons2.Remove(person);
    }

    public void Update(Person person)
    {
        context.Persons2.Update(person);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Persons2
            .AsNoTracking()
            .AnyAsync(person => person.Id == id, cancellationToken);
    }
}