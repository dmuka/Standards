using Core.Results;
using Domain.Aggregates.Common.Constants;
using Codes = Domain.Aggregates.Rooms.Constants.Codes;

namespace Domain.Aggregates.Common.ValueObjects;

public static class NameErrors
{
    public static readonly Error WrongLengthValue = Error.Problem(
        Codes.WrongLengthValue,
        $"The provided length value is wrong (less than {NameConstants.MinLength}).");
}