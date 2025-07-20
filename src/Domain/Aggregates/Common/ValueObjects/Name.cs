using Core;
using Core.Results;
using Domain.Aggregates.Common.Specifications;

namespace Domain.Aggregates.Common.ValueObjects;

public class Name : ValueObject
{
    protected Name() { }
    public string Value { get; }

    private Name(string value) => Value = value;

    public static Result<Name> Create(string name)
    {
        var nameValidationResult = new NameMustHaveValidLength(name).IsSatisfied();

        return nameValidationResult.IsFailure 
            ? Result<Name>.ValidationFailure(nameValidationResult.Error) 
            : Result.Success(new Name(name));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}