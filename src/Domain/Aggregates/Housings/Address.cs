using Core;
using Domain.Aggregates.Housings.Specifications;

namespace Domain.Aggregates.Housings;

public class Address : ValueObject
{
    /// <summary>
    /// Gets the address value.
    /// </summary>
    public string Value { get; }

    private Address(string value) => Value = value;

    public static Result<Address> Create(string address)
    {
        var addressValidationResult = new AddressMustBeValid(address).IsSatisfied();

        return addressValidationResult.IsFailure 
            ? Result<Address>.ValidationFailure(addressValidationResult.Error) 
            : Result.Success(new Address(address));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}