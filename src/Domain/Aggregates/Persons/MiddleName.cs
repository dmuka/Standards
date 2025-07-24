using Core;
using Core.Results;
using Domain.Aggregates.Persons.Specifications;

namespace Domain.Aggregates.Persons;

public class MiddleName : ValueObject
{
    protected MiddleName() { }
    public string Value { get; } = null!;

    private MiddleName(string value) => Value = value;

    public static Result<MiddleName> Create(string middleName)
    {
        var middleNameValidationResult = new MiddleNameMustBeValid(middleName).IsSatisfied();

        return middleNameValidationResult.IsFailure 
            ? Result.Failure<MiddleName>(middleNameValidationResult.Error) 
            : Result.Success(new MiddleName(middleName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}