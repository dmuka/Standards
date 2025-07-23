namespace Domain.Aggregates.Persons;

/// <summary>
/// Represents a repository for managing Person entities.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Retrieves a Person entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Person.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Person entity if found; otherwise, null.</returns>
    public Task<Person?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Person entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Persons.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Person entities.</returns>
    public Task<Person[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Person entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Person entities.</returns>
    public Task<Person[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Person entity to the repository.
    /// </summary>
    /// <param name="person">The Person entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Person person, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Person entity from the repository.
    /// </summary>
    /// <param name="person">The Person entity to remove.</param>
    public void Remove(Person person);

    /// <summary>
    /// Updates an existing Person entity in the repository.
    /// </summary>
    /// <param name="person">The Person entity to update.</param>
    public void Update(Person person);

    /// <summary>
    /// Checks if a Person entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Person.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Person exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}