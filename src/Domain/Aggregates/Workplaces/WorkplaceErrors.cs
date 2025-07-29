using Core.Results;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Workplaces.Constants;

namespace Domain.Aggregates.Workplaces;

public static class WorkplaceErrors
{
    public static Error NotFound(Guid workplaceId) => Error.NotFound(
        Codes.NotFound, 
        $"The workplace with the id = '{workplaceId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error EmptyImagePathValue = Error.Problem(
        Codes.EmptyImagePathValue,
        "Image path can't be empty.");

    public static readonly Error ImagePathLengthTooSmall = Error.Problem(
        Codes.ImagePathLengthTooSmall,
        $"Image path length too small (less than {WorkplaceConstants.ImagePathMinLength}).");

    public static Error UnsupportedImageFile(string extension) => Error.Problem(
        Codes.UnsupportedImageFile,
        $"Unsupported image file (extension: {extension}).");

    public static Error SpecifiedImageFileDoesntExist(string imagePath) => Error.Problem(
        Codes.SpecifiedImageFileDoesntExist,
        $"Image file does not exist at the specified path (path: {imagePath}).");
    
    public static Error PersonNotFound(Guid personId) => Error.NotFound(
        Codes.NotFound, 
        $"The person with the id = '{personId}' was not found.");

    public static readonly Error PersonAlreadyExist = Error.Problem(
        Codes.PersonAlreadyExist,
        "This person already exist in the workplace.");

    public static readonly Error OneOfThePersonAlreadyExist = Error.Problem(
        Codes.OneOfThePersonAlreadyExist,
        "One of the provided person already exist in the room.");
    
    public static Error StandardNotFound(Guid standardId) => Error.NotFound(
        Codes.NotFound, 
        $"The standard with the id = '{standardId}' was not found.");

    public static readonly Error StandardAlreadyExist = Error.Problem(
        Codes.StandardAlreadyExist,
        "This standard already exist in the workplace.");

    public static readonly Error OneOfTheStandardAlreadyExist = Error.Problem(
        Codes.OneOfTheStandardAlreadyExist,
        "One of the provided standard already exist in the room.");

    public static Error ThisImageAlreadySetForThisWorkplace(string imagePath) => Error.Problem(
        Codes.ThisImageAlreadySetForThisWorkplace,
        $"This image already set for this workplace (path: {imagePath}).");

    public static Error ThisPersonAlreadySetAsResponsibleForThisWorkplace(Guid responsibleId) => Error.Problem(
        Codes.ThisPersonAlreadySetAsResponsibleForThisWorkplace,
        $"This person already set as responsible for this workplace (person id: {responsibleId}).");
}