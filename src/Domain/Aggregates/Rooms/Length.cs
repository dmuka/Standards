using Core;
using Domain.Aggregates.Housings.Specifications;
using Domain.Aggregates.Rooms.Specifications;

namespace Domain.Aggregates.Rooms;

public class Length : ValueObject
{
    protected Length() { }
    public float Value { get; }

    private Length(float value) => Value = value;

    public static Result<Length> Create(float length)
    {
        var lengthValidationResult = new LengthMustBeValid(length).IsSatisfied();

        return lengthValidationResult.IsFailure 
            ? Result<Length>.ValidationFailure(lengthValidationResult.Error) 
            : Result.Success(new Length(length));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}