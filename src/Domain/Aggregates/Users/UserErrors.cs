using Core.Results;
using Domain.Aggregates.Users.Constants;

namespace Domain.Aggregates.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        Codes.NotFound, 
        $"The user with the id = '{userId}' was not found.");
    
    public static Error PersonNotFound(Guid personId) => Error.NotFound(
        Codes.NotFound, 
        $"The person with the id = '{personId}' was not found.");

    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");

    public static readonly Error PersonAlreadyExist = Error.Problem(
        Codes.PersonAlreadyExist,
        "This person already exist in the user.");
}