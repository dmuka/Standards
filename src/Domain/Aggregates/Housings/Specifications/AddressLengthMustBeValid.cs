using Core;
using Core.Results;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class AddressLengthMustBeValid(string address) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(address)) return Result<string>.ValidationFailure(HousingErrors.EmptyAddress);
        
        return address.Length switch
        {
            < HousingConstants.AddressMinLength => Result<string>.ValidationFailure(HousingErrors.TooShortAddress),
            > HousingConstants.AddressMaxLength => Result<string>.ValidationFailure(HousingErrors.TooLargeAddress),
            _ => Result.Success()
        };
    }
}