using Core;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class HousingShortNameMustBeValid(string housingShortName) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(housingShortName)) return Result.Failure<string>(HousingErrors.EmptyHousingShortName);
        
        return housingShortName.Length switch
        {
            < HousingConstants.AddressMinLength => Result.Failure<string>(HousingErrors.TooShortHousingShortName),
            > HousingConstants.AddressMaxLength => Result.Failure<string>(HousingErrors.TooLargeHousingShortName),
            _ => Result.Success()
        };
    }
}