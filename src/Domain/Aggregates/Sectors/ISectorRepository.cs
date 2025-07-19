namespace Domain.Aggregates.Sectors;

/// <summary>
/// Represents a repository for managing Sector entities.
/// </summary>
public interface ISectorRepository
{
    /// <summary>
    /// Retrieves a Sector entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Sector.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Sector entity if found; otherwise, null.</returns>
    public Task<Sector?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Sector entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Sectors.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Sector entities.</returns>
    public Task<Sector[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Sector entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Sector entities.</returns>
    public Task<Sector[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Sector entity to the repository.
    /// </summary>
    /// <param name="room">The Sector entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Sector room, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Sector entity from the repository.
    /// </summary>
    /// <param name="room">The Sector entity to remove.</param>
    public void Remove(Sector room);

    /// <summary>
    /// Updates an existing Sector entity in the repository.
    /// </summary>
    /// <param name="room">The Sector entity to update.</param>
    public void Update(Sector room);

    /// <summary>
    /// Checks if a Sector entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Sector.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Sector exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}