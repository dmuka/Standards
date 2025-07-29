using System.Linq.Expressions;

namespace Domain.Aggregates.Workplaces;

/// <summary>
/// Represents a repository for managing Workplace entities.
/// </summary>
public interface IWorkplaceRepository
{
    /// <summary>
    /// Retrieves a Workplace entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Workplace.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Workplace entity if found; otherwise, null.</returns>
    public Task<Workplace?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Workplace entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Workplaces.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Workplace entities.</returns>
    public Task<Workplace[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Workplace entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Workplace entities.</returns>
    public Task<Workplace[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Workplace entity to the repository.
    /// </summary>
    /// <param name="workplace">The Workplace entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Workplace workplace, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Workplace entity from the repository.
    /// </summary>
    /// <param name="workplace">The Workplace entity to remove.</param>
    public void Remove(Workplace workplace);

    /// <summary>
    /// Updates an existing Workplace entity in the repository.
    /// </summary>
    /// <param name="workplace">The Workplace entity to update.</param>
    public void Update(Workplace workplace);

    /// <summary>
    /// Checks if a Workplace entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Workplace.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Workplace exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a queryable collection of Workplace entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to test each Workplace entity for a condition.</param>
    /// <returns>An IQueryable collection of Workplace entities that satisfy the specified predicate.</returns>
    public IQueryable<Workplace> Where(Expression<Func<Workplace, bool>> predicate);
}