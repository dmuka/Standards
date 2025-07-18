namespace Domain.Aggregates.Floors;

/// <summary>
/// Represents a repository for managing Floor entities.
/// </summary>
public interface IFloorRepository
{
    /// <summary>
    /// Retrieves a Floor entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Floor.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Floor entity if found; otherwise, null.</returns>
    public Task<Floor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Floor entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Floors.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Floor entities.</returns>
    public Task<Floor[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Floor entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Floor entities.</returns>
    public Task<Floor[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Floor entity to the repository.
    /// </summary>
    /// <param name="floor">The Floor entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Floor floor, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Floor entity from the repository.
    /// </summary>
    /// <param name="floor">The Floor entity to remove.</param>
    public void Remove(Floor floor);

    /// <summary>
    /// Updates an existing Floor entity in the repository.
    /// </summary>
    /// <param name="floor">The Floor entity to update.</param>
    public void Update(Floor floor);

    /// <summary>
    /// Checks if a Floor entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Floor.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Floor exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}