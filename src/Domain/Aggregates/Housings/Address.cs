using Core;
using Core.Results;
using Domain.Aggregates.Housings.Specifications;

namespace Domain.Aggregates.Housings;

public class Address : ValueObject
{
    protected Address() { }
    /// <summary>
    /// Gets the address value.
    /// </summary>
    public string Value { get; } = null!;

    private Address(string value) => Value = value;

    public static Result<Address> Create(string address)
    {
        var addressValidationResult = new AddressLengthMustBeValid(address).IsSatisfied();

        return addressValidationResult.IsFailure 
            ? Result<Address>.ValidationFailure(addressValidationResult.Error) 
            : Result.Success(new Address(address));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}