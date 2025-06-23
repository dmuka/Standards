using Core;
using Domain.Aggregates.Floors.Constants;

namespace Domain.Aggregates.Floors;

public static class FloorErrors
{
    public static Error NotFound(Guid floorId) => Error.NotFound(
        Codes.NotFound, 
        $"The floor with the id = '{floorId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error InvalidFloorCount = Error.Problem(
        Codes.InvalidFloorCount,
        "The provided floor count value is less than 1.");

    public static readonly Error FloorAlreadyExist = Error.Problem(
        Codes.FloorAlreadyExist,
        "The provided floor count value is less than 1.");
}