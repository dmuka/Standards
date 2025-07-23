using Core;
using Core.Results;
using Domain.Aggregates.Persons.Specifications;

namespace Domain.Aggregates.Persons;

public class BirthdayDate : ValueObject
{
    protected BirthdayDate() { }
    public DateOnly Value { get; }

    private BirthdayDate(DateOnly value) => Value = value;

    public static Result<BirthdayDate> Create(DateOnly birthdayDate)
    {
        var birthdayDateValidationResult = new BirthdayDateMustBeValid(birthdayDate).IsSatisfied();

        return birthdayDateValidationResult.IsFailure 
            ? Result.Failure<BirthdayDate>(birthdayDateValidationResult.Error) 
            : Result.Success(new BirthdayDate(birthdayDate));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}