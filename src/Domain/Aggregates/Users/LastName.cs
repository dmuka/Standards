using Core;
using Core.Results;
using Domain.Aggregates.Persons.Specifications;

namespace Domain.Aggregates.Users;

public class LastName : ValueObject
{
    protected LastName() { }
    public string Value { get; } = null!;

    private LastName(string value) => Value = value;

    public static Result<LastName> Create(string lastName)
    {
        var lastNameValidationResult = new LastNameMustBeValid(lastName).IsSatisfied();

        return lastNameValidationResult.IsFailure 
            ? Result.Failure<LastName>(lastNameValidationResult.Error) 
            : Result.Success(new LastName(lastName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}