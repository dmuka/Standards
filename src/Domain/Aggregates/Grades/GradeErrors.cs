using Core.Results;
using Domain.Aggregates.Grades.Constants;

namespace Domain.Aggregates.Grades;

public static class GradeErrors
{
    public static Error NotFound(Guid gradeId) => Error.NotFound(
        Codes.NotFound, 
        $"The grade with the id = '{gradeId}' was not found.");
    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");
}