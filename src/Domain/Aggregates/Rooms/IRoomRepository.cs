namespace Domain.Aggregates.Rooms;

/// <summary>
/// Represents a repository for managing Room entities.
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// Retrieves a Room entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Room.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Room entity if found; otherwise, null.</returns>
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Room entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Rooms.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Room entities.</returns>
    public Task<Room[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Room entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Room entities.</returns>
    public Task<Room[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Room entity to the repository.
    /// </summary>
    /// <param name="room">The Room entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Room room, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Room entity from the repository.
    /// </summary>
    /// <param name="room">The Room entity to remove.</param>
    public void Remove(Room room);

    /// <summary>
    /// Updates an existing Room entity in the repository.
    /// </summary>
    /// <param name="room">The Room entity to update.</param>
    public void Update(Room room);

    /// <summary>
    /// Checks if a Room entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Room.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Room exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}