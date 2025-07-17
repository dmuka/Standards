using Core.Results;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms;

public static class RoomErrors
{
    public static Error NotFound(Guid roomId) => Error.NotFound(
        Codes.NotFound, 
        $"The room with the id = '{roomId}' was not found.");
    
    public static Error PersonNotFound(Guid personId) => Error.NotFound(
        Codes.NotFound, 
        $"The person with the id = '{personId}' was not found.");
    
    public static Error WorkplaceNotFound(Guid workplaceId) => Error.NotFound(
        Codes.NotFound, 
        $"The workplace with the id = '{workplaceId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error WrongLengthValue = Error.Problem(
        Codes.WrongLengthValue,
        $"The provided length value is wrong (less than {RoomConstants.MinLength}).");

    public static readonly Error WrongHeightValue = Error.Problem(
        Codes.WrongHeightValue,
        $"The provided height value is wrong (less than {RoomConstants.MinHeight}).");

    public static readonly Error WrongWidthValue = Error.Problem(
        Codes.WrongWidthValue,
        $"The provided width value is wrong (less than {RoomConstants.MinWidth}).");

    public static readonly Error PersonAlreadyExist = Error.Problem(
        Codes.PersonAlreadyExist,
        $"This person already exist in the room.");

    public static readonly Error OneOfThePersonAlreadyExist = Error.Problem(
        Codes.OneOfThePersonAlreadyExist,
        $"One of the provided person already exist in the room.");

    public static readonly Error WorkplaceAlreadyExist = Error.Problem(
        Codes.WorkplaceAlreadyExist,
        $"This workplace already exist in the room.");

    public static readonly Error OneOfTheWorkplaceAlreadyExist = Error.Problem(
        Codes.OneOfTheWorkplaceAlreadyExist,
        $"One of the provided workplace already exist in the room.");
}