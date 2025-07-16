using Core;
using Domain.Aggregates.Housings.Specifications;

namespace Domain.Aggregates.Housings;

public class HousingName : ValueObject
{
    protected HousingName() { }

    /// <summary>
    /// Gets the housing name value.
    /// </summary>
    public string Value { get; } = null!;

    private HousingName(string value) => Value = value;

    public static Result<HousingName> Create(string housingName)
    {
        var housingNameValidationResult = new HousingNameMustBeValid(housingName).IsSatisfied();

        return housingNameValidationResult.IsFailure 
            ? Result<HousingName>.ValidationFailure(housingNameValidationResult.Error) 
            : Result.Success(new HousingName(housingName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}