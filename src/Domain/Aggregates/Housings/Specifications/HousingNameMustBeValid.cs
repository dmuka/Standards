using Core;
using Core.Results;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class HousingNameMustBeValid(string housingName) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(housingName)) return Result.Failure<string>(HousingErrors.EmptyHousingName);
        
        return housingName.Length switch
        {
            < HousingConstants.HousingNameMinLength => Result.Failure<string>(HousingErrors.TooShortHousingName),
            > HousingConstants.HousingNameMaxLength => Result.Failure<string>(HousingErrors.TooLargeHousingName),
            _ => Result.Success()
        };
    }
}