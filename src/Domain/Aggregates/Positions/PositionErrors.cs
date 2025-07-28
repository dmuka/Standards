using Core.Results;
using Domain.Aggregates.Positions.Constants;

namespace Domain.Aggregates.Positions;

public static class PositionErrors
{
    public static Error NotFound(Guid positionId) => Error.NotFound(
        Codes.NotFound, 
        $"The position with the id = '{positionId}' was not found.");
    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");
}