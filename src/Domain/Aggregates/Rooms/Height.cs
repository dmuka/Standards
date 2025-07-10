using Core;
using Domain.Aggregates.Housings.Specifications;
using Domain.Aggregates.Rooms.Specifications;

namespace Domain.Aggregates.Rooms;

public class Height : ValueObject
{
    protected Height() { }
    public float Value { get; }

    private Height(float value) => Value = value;

    public static Result<Height> Create(float height)
    {
        var heightValidationResult = new HeightMustBeValid(height).IsSatisfied();

        return heightValidationResult.IsFailure 
            ? Result<Height>.ValidationFailure(heightValidationResult.Error) 
            : Result.Success(new Height(height));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}