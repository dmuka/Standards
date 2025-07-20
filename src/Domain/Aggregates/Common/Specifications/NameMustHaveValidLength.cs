using Core;
using Core.Results;
using Domain.Aggregates.Common.Constants;
using Domain.Aggregates.Common.ValueObjects;

namespace Domain.Aggregates.Common.Specifications;

public class NameMustHaveValidLength(string name) : ISpecification
{
    public Result IsSatisfied()
    {
        return name.Length is < NameConstants.MinLength or > NameConstants.MaxLength
                ? Result.Failure<string>(NameErrors.WrongLengthValue)
                : Result.Success();
    }
}