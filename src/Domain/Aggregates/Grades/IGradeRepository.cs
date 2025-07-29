using System.Linq.Expressions;

namespace Domain.Aggregates.Grades;

/// <summary>
/// Represents a repository for managing Grade entities.
/// </summary>
public interface IGradeRepository
{
    /// <summary>
    /// Retrieves a Grade entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Grade.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Grade entity if found; otherwise, null.</returns>
    public Task<Grade?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Grade entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Categorys.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Grade entities.</returns>
    public Task<Grade[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Grade entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Grade entities.</returns>
    public Task<Grade[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Grade entity to the repository.
    /// </summary>
    /// <param name="category">The Grade entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Grade category, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Grade entity from the repository.
    /// </summary>
    /// <param name="category">The Grade entity to remove.</param>
    public void Remove(Grade category);

    /// <summary>
    /// Updates an existing Grade entity in the repository.
    /// </summary>
    /// <param name="category">The Grade entity to update.</param>
    public void Update(Grade category);

    /// <summary>
    /// Checks if a Grade entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Grade.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Grade exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a queryable collection of Grade entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to test each Grade entity for a condition.</param>
    /// <returns>An IQueryable collection of Grade entities that satisfy the specified predicate.</returns>
    public IQueryable<Grade> Where(Expression<Func<Grade, bool>> predicate);
}