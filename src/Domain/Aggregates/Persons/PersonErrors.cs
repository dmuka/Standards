using Core.Results;
using Domain.Aggregates.Persons.Constants;
using Codes = Domain.Aggregates.Persons.Constants.Codes;

namespace Domain.Aggregates.Persons;

public static class PersonErrors
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

    public static readonly Error WrongFirstNameValue = Error.Problem(
        Codes.WrongFirstNameValue,
        $"The provided first name value is wrong (less than {PersonConstants.MinFirstNameLength} or greater than {PersonConstants.MaxFirstNameLength}).");
    public static readonly Error WrongMiddleNameValue = Error.Problem(
        Codes.WrongMiddleNameValue,
        $"The provided middle name value is wrong (less than {PersonConstants.MinMiddleNameLength} or greater than {PersonConstants.MaxMiddleNameLength}).");
    public static readonly Error WrongLastNameValue = Error.Problem(
        Codes.WrongLastNameValue,
        $"The provided last name value is wrong (less than {PersonConstants.MinLastNameLength} or greater than {PersonConstants.MaxLastNameLength}).");
    public static readonly Error WrongAgeValue = Error.Problem(
        Codes.WrongAgeValue,
        $"The provided birthday date value is wrong (age less than {PersonConstants.MinAge} or greater than {PersonConstants.MaxAge}).");

    public static readonly Error WorkplaceAlreadyExist = Error.Problem(
        Codes.WorkplaceAlreadyExist,
        $"This workplace already exist in the room.");

    public static readonly Error OneOfTheWorkplaceAlreadyExist = Error.Problem(
        Codes.OneOfTheWorkplaceAlreadyExist,
        $"One of the provided workplace already exist in the room.");

    public static readonly Error ThisPositionAlreadySetForThisPerson = Error.Problem(
        Codes.ThisPositionAlreadySetForThisPerson,
        $"This position already set for this person.");

    public static readonly Error ThisSectorAlreadySetForThisPerson = Error.Problem(
        Codes.ThisSectorAlreadySetForThisPerson,
        $"This sector already set for this person.");
}