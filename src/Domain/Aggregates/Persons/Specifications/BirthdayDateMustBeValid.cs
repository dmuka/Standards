using Core;
using Core.Results;
using Domain.Aggregates.Persons.Constants;

namespace Domain.Aggregates.Persons.Specifications;

public class BirthdayDateMustBeValid(DateOnly birthdayDate) : ISpecification
{
    public Result IsSatisfied()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthdayDate.Year;
        if (birthdayDate > today.AddYears(-age)) age--;
        
        return age is < PersonConstants.MinAge or > PersonConstants.MaxAge
                ? Result<BirthdayDate>.ValidationFailure(PersonErrors.WrongAgeValue)
                : Result.Success();
    }
}