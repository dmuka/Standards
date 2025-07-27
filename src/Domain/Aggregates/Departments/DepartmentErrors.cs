using Core.Results;
using Domain.Aggregates.Departments.Constants;

namespace Domain.Aggregates.Departments;

public static class DepartmentErrors
{
    public static Error NotFound(Guid departmentId) => Error.NotFound(
        Codes.NotFound, 
        $"The department with the id = '{departmentId}' was not found.");
    
    public static Error SectorNotFound(Guid sectorId) => Error.NotFound(
        Codes.NotFound, 
        $"The sector with the id = '{sectorId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error SectorAlreadyExist = Error.Problem(
        Codes.SectorAlreadyExist,
        "This sector already exist in the department.");
}