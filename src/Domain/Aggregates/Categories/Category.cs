using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Categories;

public class Category : NamedAggregateRoot<CategoryId>, ICacheable
{
    protected Category() { }

    private Category(
        Name name,
        ShortName shortName,
        CategoryId categoryId,
        string? comments)
    {
        Id = categoryId;
        Name = name;
        ShortName = shortName;
        Comments = comments;
    }

    public static Result<Category> Create(
        string categoryName,
        string shortCategoryName,
        Guid? categoryId = null,
        string? comments = null)
    {
        var validationResults = ValidateCategoryDetails(categoryName, shortCategoryName);
        if (validationResults.Length != 0)
            return Result<Category>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var category = new Category(
            Name.Create(categoryName).Value,
            ShortName.Create(shortCategoryName).Value,
            categoryId is null ? new CategoryId(Guid.CreateVersion7()) : new CategoryId(categoryId.Value),
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