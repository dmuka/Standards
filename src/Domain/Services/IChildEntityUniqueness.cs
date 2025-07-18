using Core;

namespace Domain.Services;

/// <summary>
/// Provides functionality to check the uniqueness of a child entity within a parent entity.
/// </summary>
public interface IChildEntityUniqueness
{
    /// <summary>
    /// Determines whether a child entity is unique within the context of a parent entity.
    /// </summary>
    /// <typeparam name="TChild">The type of the child entity.</typeparam>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <param name="childEntityId">The unique identifier of the child entity.</param>
    /// <param name="parentEntityId">The unique identifier of the parent entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the child entity is unique within the parent entity.</returns>
    Task<bool> IsUniqueAsync<TChild, TParent>(TypedId childEntityId, TypedId parentEntityId, CancellationToken cancellationToken)
        where TChild : Entity
        where TParent : Entity;
}