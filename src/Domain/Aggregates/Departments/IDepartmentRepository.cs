using System.Linq.Expressions;

namespace Domain.Aggregates.Departments;

/// <summary>
/// Represents a repository for managing Department entities.
/// </summary>
public interface IDepartmentRepository
{
    /// <summary>
    /// Retrieves a Department entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Department entity if found; otherwise, null.</returns>
    public Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Department entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Departments.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Department entities.</returns>
    public Task<Department[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Department entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Department entities.</returns>
    public Task<Department[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Department entity to the repository.
    /// </summary>
    /// <param name="department">The Department entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Department department, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Department entity from the repository.
    /// </summary>
    /// <param name="department">The Department entity to remove.</param>
    public void Remove(Department department);

    /// <summary>
    /// Updates an existing Department entity in the repository.
    /// </summary>
    /// <param name="department">The Department entity to update.</param>
    public void Update(Department department);

    /// <summary>
    /// Checks if a Department entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Department.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Department exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a queryable collection of Department entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to test each Department entity for a condition.</param>
    /// <returns>An IQueryable collection of Department entities that satisfy the specified predicate.</returns>
    public IQueryable<Department> Where(Expression<Func<Department, bool>> predicate);
}