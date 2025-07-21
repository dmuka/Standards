using Core.Results;

namespace Core;

public interface ISpecification
{
    Result IsSatisfied();
}