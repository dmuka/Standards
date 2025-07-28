using System.Linq.Expressions;

namespace Domain.Aggregates.Categories;

/// <summary>
/// Represents a repository for managing Category entities.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves a Category entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Category.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Category entity if found; otherwise, null.</returns>
    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves multiple Category entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the Categorys.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of Category entities.</returns>
    public Task<Category[]> GetByIdsAsync(Guid[] ids, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all Category entities.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of all Category entities.</returns>
    public Task<Category[]> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Category entity to the repository.
    /// </summary>
    /// <param name="category">The Category entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddAsync(Category category, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a Category entity from the repository.
    /// </summary>
    /// <param name="category">The Category entity to remove.</param>
    public void Remove(Category category);

    /// <summary>
    /// Updates an existing Category entity in the repository.
    /// </summary>
    /// <param name="category">The Category entity to update.</param>
    public void Update(Category category);

    /// <summary>
    /// Checks if a Category entity exists in the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Category.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the Category exists; otherwise, false.</returns>
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a queryable collection of Category entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to test each Category entity for a condition.</param>
    /// <returns>An IQueryable collection of Category entities that satisfy the specified predicate.</returns>
    public IQueryable<Category> Where(Expression<Func<Category, bool>> predicate);
}