using Core;
using Domain.Aggregates.Housings.Constants;
using Domain.Housings.Constants;

namespace Domain.Aggregates.Housings;

public static class HousingErrors
{
    public static Error NotFound(Guid housingId) => Error.NotFound(
        Codes.NotFound, 
        $"The housing with the id = '{housingId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error EmptyAddress = Error.Problem(
        Codes.EmptyAddressAddress,
        "The provided address value is empty.");

    public static readonly Error TooShortAddress = Error.Problem(
        Codes.TooShortAddress,
        $"The provided address value is too short (less than {HousingConstants.AddressMinLength}).");

    public static readonly Error TooLargeAddress = Error.Problem(
        Codes.TooLargeAddress,
        $"The provided address value is too large (greater than {HousingConstants.AddressMaxLength}).");

    public static readonly Error InvalidFloorCount = Error.Problem(
        Codes.InvalidFloorCount,
        "The provided floor count value is less than 1.");
}