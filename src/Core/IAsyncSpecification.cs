using Core.Results;

namespace Core;

public interface IAsyncSpecification
{
    Task<Result> IsSatisfiedAsync(CancellationToken cancellationToken);
}