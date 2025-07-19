using Core.Results;
using Domain.Aggregates.Sectors.Constants;

namespace Domain.Aggregates.Sectors;

public static class SectorErrors
{
    public static Error NotFound(Guid sectorId) => Error.NotFound(
        Codes.NotFound, 
        $"The sector with the id = '{sectorId}' was not found.");
    
    public static Error PersonNotFound(Guid personId) => Error.NotFound(
        Codes.NotFound, 
        $"The person with the id = '{personId}' was not found.");
    
    public static Error WorkplaceNotFound(Guid workplaceId) => Error.NotFound(
        Codes.NotFound, 
        $"The workplace with the id = '{workplaceId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

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

    public static readonly Error ThisDepartmentAlreadySetForThisSector = Error.Problem(
        Codes.ThisDepartmentAlreadySetForThisSector,
        $"This department already set for this sector.");
}