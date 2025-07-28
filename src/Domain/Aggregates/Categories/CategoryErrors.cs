using Core.Results;
using Domain.Aggregates.Categories.Constants;

namespace Domain.Aggregates.Categories;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        Codes.NotFound, 
        $"The category with the id = '{categoryId}' was not found.");
    public static Error Unauthorized() => Error.Failure(
        Codes.Unauthorized,
        "You are not authorized to perform this action.");
}