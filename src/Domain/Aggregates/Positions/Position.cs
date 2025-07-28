using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Positions;

public class Position : NamedAggregateRoot<PositionId>, ICacheable
{
    protected Position() { }

    private Position(
        Name name,
        ShortName shortName,
        PositionId positionId,
        string? comments)
    {
        Id = positionId;
        Name = name;
        ShortName = shortName;
        Comments = comments;
    }

    public static Result<Position> Create(
        string positionName,
        string shortPositionName,
        Guid? positionId = null,
        string? comments = null)
    {
        var validationResults = ValidatePositionDetails(positionName, shortPositionName);
        if (validationResults.Length != 0)
            return Result<Position>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var position = new Position(
            Name.Create(positionName).Value,
            ShortName.Create(shortPositionName).Value,
            positionId is null ? new PositionId(Guid.CreateVersion7()) : new PositionId(positionId.Value),
            comments);
            
        return Result.Success(position);
    }
    
    public static string GetCacheKey()
    {
        return Cache.Categories;
    }

    /// <summary>
    /// Validates position details.
    /// </summary>
    private static Result[] ValidatePositionDetails(string positionName, string shortPositionName)
    {
        var validationResults = new []
        {
            new NameMustHaveValidLength(positionName).IsSatisfied(),
            new ShortNameMustHaveValidLength(shortPositionName).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}