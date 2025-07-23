using Core;
using Core.Results;
using Domain.Aggregates.Persons.Constants;

namespace Domain.Aggregates.Persons.Specifications;

public class MiddleNameMustBeValid(string middleName) : ISpecification
{
    public Result IsSatisfied()
    {
        return middleName.Length is < PersonConstants.MinMiddleNameLength or > PersonConstants.MaxMiddleNameLength
            ? Result<LastName>.ValidationFailure(PersonErrors.WrongMiddleNameValue)
            : Result.Success();
    }
}