using Core.Results;
using Domain.Aggregates.Housings.Constants;

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
        Codes.EmptyAddress,
        "The provided address value is empty.");

    public static readonly Error EmptyHousingName = Error.Problem(
        Codes.EmptyHousingName,
        "The provided housing name value is empty.");

    public static readonly Error EmptyHousingShortName = Error.Problem(
        Codes.EmptyHousingShortName,
        "The provided housing short name value is empty.");

    public static readonly Error TooShortAddress = Error.Problem(
        Codes.TooShortAddress,
        $"The provided address value is too short (less than {HousingConstants.AddressMinLength}).");

    public static readonly Error TooShortHousingName = Error.Problem(
        Codes.TooShortHousingName,
        $"The provided housing name value is too short (less than {HousingConstants.HousingNameMinLength}).");

    public static readonly Error TooShortHousingShortName = Error.Problem(
        Codes.TooShortHousingShortName,
        $"The provided housing short name value is too short (less than {HousingConstants.HousingShortNameMinLength}).");

    public static readonly Error TooLargeAddress = Error.Problem(
        Codes.TooLargeAddress,
        $"The provided address value is too large (greater than {HousingConstants.AddressMaxLength}).");

    public static readonly Error TooLargeHousingName = Error.Problem(
        Codes.TooLargeHousingName,
        $"The provided housing name value is too large (greater than {HousingConstants.HousingNameMaxLength}).");

    public static readonly Error TooLargeHousingShortName = Error.Problem(
        Codes.TooLargeHousingShortName,
        $"The provided housing short name value is too large (greater than {HousingConstants.HousingShortNameMaxLength}).");

    public static readonly Error InvalidFloorCount = Error.Problem(
        Codes.InvalidFloorCount,
        "The provided floor count value is less than 1.");
}