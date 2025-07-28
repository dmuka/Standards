using System.Linq.Expressions;

namespace Domain.Aggregates.Positions;

/// <summary>
/// Represents a repository for managing Position entities.
/// </summary>
public interface IPositionRepository
{
    /// <summary>
    /// Retrieves a Position entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Position.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Position entity if found; otherwise, null.</returns>
    public Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Position entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Categorys.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Position entities.</returns>
    public Task<Position[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Position entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Position entities.</returns>
    public Task<Position[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Position entity to the repository.
    /// </summary>
    /// <param name="position">The Position entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Position position, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Position entity from the repository.
    /// </summary>
    /// <param name="position">The Position entity to remove.</param>
    public void Remove(Position position);

    /// <summary>
    /// Updates an existing Position entity in the repository.
    /// </summary>
    /// <param name="position">The Position entity to update.</param>
    public void Update(Position position);

    /// <summary>
    /// Checks if a Position entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Position.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Position exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a queryable collection of Position entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to test each Position entity for a condition.</param>
    /// <returns>An IQueryable collection of Position entities that satisfy the specified predicate.</returns>
    public IQueryable<Position> Where(Expression<Func<Position, bool>> predicate);
}