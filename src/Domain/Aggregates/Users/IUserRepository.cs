namespace Domain.Aggregates.Users;

/// <summary>
/// Represents a repository for managing User entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a User entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the User.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the User entity if found; otherwise, null.</returns>
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple User entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Users.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of User entities.</returns>
    public Task<User[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all User entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all User entities.</returns>
    public Task<User[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new User entity to the repository.
    /// </summary>
    /// <param name="user">The User entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a User entity from the repository.
    /// </summary>
    /// <param name="user">The User entity to remove.</param>
    public void Remove(User user);

    /// <summary>
    /// Updates an existing User entity in the repository.
    /// </summary>
    /// <param name="user">The User entity to update.</param>
    public void Update(User user);

    /// <summary>
    /// Checks if a User entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the User.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the User exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}