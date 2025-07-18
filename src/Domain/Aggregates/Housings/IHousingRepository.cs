namespace Domain.Aggregates.Housings
{
    /// <summary>
    /// Represents a repository for managing Housing entities.
    /// </summary>
    public interface IHousingRepository
    {
        /// <summary>
        /// Retrieves a Housing entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Housing.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the Housing entity if found; otherwise, null.</returns>
        public Task<Housing?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves multiple Housing entities by their unique identifiers.
        /// </summary>
        /// <param name="ids">An array of unique identifiers for the Housings.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of Housing entities.</returns>
        public Task<Housing[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all Housing entities.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Housing entities.</returns>
        public Task<Housing[]> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new Housing entity to the repository.
        /// </summary>
        /// <param name="housing">The Housing entity to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddAsync(Housing housing, CancellationToken cancellationToken);

        /// <summary>
        /// Removes a Housing entity from the repository.
        /// </summary>
        /// <param name="floor">The Housing entity to remove.</param>
        public void Remove(Housing floor);

        /// <summary>
        /// Updates an existing Housing entity in the repository.
        /// </summary>
        /// <param name="floor">The Housing entity to update.</param>
        public void Update(Housing floor);

        /// <summary>
        /// Checks if a Housing entity exists in the repository by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Housing.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the Housing exists; otherwise, false.</returns>
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}