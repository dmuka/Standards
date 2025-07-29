using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Grades;

public class Grade : NamedAggregateRoot<GradeId>, ICacheable
{
    protected Grade() { }

    private Grade(
        Name name,
        ShortName shortName,
        GradeId categoryId,
        string? comments)
    {
        Id = categoryId;
        Name = name;
        ShortName = shortName;
        Comments = comments;
    }

    public static Result<Grade> Create(
        string categoryName,
        string shortCategoryName,
        Guid? categoryId = null,
        string? comments = null)
    {
        var validationResults = ValidateCategoryDetails(categoryName, shortCategoryName);
        if (validationResults.Length != 0)
            return Result<Grade>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var category = new Grade(
            Name.Create(categoryName).Value,
            ShortName.Create(shortCategoryName).Value,
            categoryId is null ? new GradeId(Guid.CreateVersion7()) : new GradeId(categoryId.Value),
            comments);
            
        return Result.Success(category);
    }
    
    public static string GetCacheKey()
    {
        return Cache.Categories;
    }

    /// <summary>
    /// Validates category details.
    /// </summary>
    private static Result[] ValidateCategoryDetails(string categoryName, string shortCategoryName)
    {
        var validationResults = new []
        {
            new NameMustHaveValidLength(categoryName).IsSatisfied(),
            new ShortNameMustHaveValidLength(shortCategoryName).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}