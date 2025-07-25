using Core;
using Core.Results;
using Domain.Aggregates.Housings.Specifications;

namespace Domain.Aggregates.Housings;

public class HousingShortName : ValueObject
{
    protected HousingShortName() { }
    /// <summary>
    /// Gets the housing short name value.
    /// </summary>
    public string? Value { get; }

    private HousingShortName(string? value) => Value = value;

    public static Result<HousingShortName> Create(string? housingShortName)
    {
        var housingShortNameValidationResult = new HousingShortNameLengthMustBeValid(housingShortName).IsSatisfied();

        return housingShortNameValidationResult.IsFailure 
            ? Result<HousingShortName>.ValidationFailure(housingShortNameValidationResult.Error) 
            : Result.Success(new HousingShortName(housingShortName));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value!;
    }
}