using Core;
using Core.Results;
using Domain.Aggregates.Housings.Constants;

namespace Domain.Aggregates.Housings.Specifications;

public class AddressMustBeValid(string address) : ISpecification
{
    public Result IsSatisfied()
    {
        if (string.IsNullOrEmpty(address)) return Result.Failure<string>(HousingErrors.EmptyAddress);
        
        return address.Length switch
        {
            < HousingConstants.AddressMinLength => Result.Failure<string>(HousingErrors.TooShortAddress),
            > HousingConstants.AddressMaxLength => Result.Failure<string>(HousingErrors.TooLargeAddress),
            _ => Result.Success()
        };
    }
}