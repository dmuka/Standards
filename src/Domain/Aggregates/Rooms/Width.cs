using Core;
using Core.Results;
using Domain.Aggregates.Rooms.Specifications;

namespace Domain.Aggregates.Rooms;

public class Width : ValueObject
{
    protected Width() { }
    public float Value { get; }

    private Width(float value) => Value = value;

    public static Result<Width> Create(float width)
    {
        var lengthValidationResult = new WidthMustBeValid(width).IsSatisfied();

        return lengthValidationResult.IsFailure 
            ? Result<Width>.ValidationFailure(lengthValidationResult.Error) 
            : Result.Success(new Width(width));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}