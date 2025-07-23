using Core;
using Core.Results;
using Domain.Aggregates.Persons.Constants;

namespace Domain.Aggregates.Persons.Specifications;

public class LastNameMustBeValid(string lastName) : ISpecification
{
    public Result IsSatisfied()
    {
        return lastName.Length is < PersonConstants.MinLastNameLength or > PersonConstants.MaxLastNameLength
            ? Result<LastName>.ValidationFailure(PersonErrors.WrongLastNameValue)
            : Result.Success();
    }
}