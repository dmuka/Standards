using Core;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class HousingNameMustBeValid(string housingName) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(housingName)) return Result.Failure<string>(HousingErrors.EmptyHousingName);
        
        return housingName.Length switch
        {
            < HousingConstants.AddressMinLength => Result.Failure<string>(HousingErrors.TooShortHousingName),
            > HousingConstants.AddressMaxLength => Result.Failure<string>(HousingErrors.TooLargeHousingName),
            _ => Result.Success()
        };
    }
}