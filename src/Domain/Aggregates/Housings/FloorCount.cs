using Core;
using Domain.Aggregates.Housings.Specifications;

namespace Domain.Aggregates.Housings;

public class FloorCount : ValueObject
{
    /// <summary>
    /// Gets the floor count value.
    /// </summary>
    public int Value { get; }

    private FloorCount(int value) => Value = value;

    public static Result<FloorCount> Create(int floorCount)
    {
        var floorCountValidationResult = new FloorCountMustBeValid(floorCount).IsSatisfied();

        return floorCountValidationResult.IsFailure 
            ? Result<FloorCount>.ValidationFailure(floorCountValidationResult.Error) 
            : Result.Success(new FloorCount(floorCount));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}