using Core;
using Core.Results;
using Domain.Aggregates.Common.Constants;
using Domain.Aggregates.Common.ValueObjects;

namespace Domain.Aggregates.Common.Specifications;

public class ShortNameMustHaveValidLength(string shortName) : ISpecification
{
    public Result IsSatisfied()
    {
        return shortName.Length is < ShortNameConstants.MinLength or > ShortNameConstants.MaxLength
                ? Result.Failure<string>(NameErrors.WrongLengthValue)
                : Result.Success();
    }
}