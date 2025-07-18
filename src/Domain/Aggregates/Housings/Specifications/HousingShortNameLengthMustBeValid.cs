using Core;
using Core.Results;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class HousingShortNameLengthMustBeValid(string housingShortName) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(housingShortName)) return Result.Failure<string>(HousingErrors.EmptyHousingShortName);
        
        return housingShortName.Length switch
        {
            < HousingConstants.HousingShortNameMinLength => Result.Failure<string>(HousingErrors.TooShortHousingShortName),
            > HousingConstants.HousingShortNameMaxLength => Result.Failure<string>(HousingErrors.TooLargeHousingShortName),
            _ => Result.Success()
        };
    }
}