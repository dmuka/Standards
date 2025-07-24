using Core;
using Core.Results;
using Domain.Aggregates.Persons.Specifications;

namespace Domain.Aggregates.Users;

public class FirstName : ValueObject
{
    protected FirstName() { }
    public string Value { get; } = null!;

    private FirstName(string value) => Value = value;

    public static Result<FirstName> Create(string firstName)
    {
        var firstNameValidationResult = new FirstNameMustBeValid(firstName).IsSatisfied();

        return firstNameValidationResult.IsFailure 
            ? Result.Failure<FirstName>(firstNameValidationResult.Error) 
            : Result.Success(new FirstName(firstName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}