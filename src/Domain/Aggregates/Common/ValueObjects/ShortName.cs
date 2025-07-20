using Core;
using Core.Results;
using Domain.Aggregates.Common.Specifications;

namespace Domain.Aggregates.Common.ValueObjects;

public class ShortName : ValueObject
{
    protected ShortName() { }
    public string Value { get; }

    private ShortName(string value) => Value = value;

    public static Result<ShortName> Create(string shortName)
    {
        var shortNameValidationResult = new ShortNameMustHaveValidLength(shortName).IsSatisfied();

        return shortNameValidationResult.IsFailure 
            ? Result<ShortName>.ValidationFailure(shortNameValidationResult.Error) 
            : Result.Success(new ShortName(shortName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}