using Core;
using Core.Results;
using Domain.Aggregates.Persons.Constants;

namespace Domain.Aggregates.Persons.Specifications;

public class FirstNameMustBeValid(string firstName) : ISpecification
{
    public Result IsSatisfied()
    {
        return firstName.Length is < PersonConstants.MinFirstNameLength or > PersonConstants.MaxFirstNameLength
                ? Result<FirstName>.ValidationFailure(PersonErrors.WrongFirstNameValue)
                : Result.Success();
    }
}