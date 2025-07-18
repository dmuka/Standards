using Core;

namespace Domain.Services;

public interface IChildEntityUniqueness
{
    Task<bool> IsUniqueAsync<TChild, TParent>(TypedId childEntityId, TypedId parentEntityId, CancellationToken cancellationToken)
        where TChild : Entity
        where TParent : Entity;
}