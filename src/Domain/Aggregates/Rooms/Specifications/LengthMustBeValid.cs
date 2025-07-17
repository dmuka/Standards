using Core;
using Core.Results;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms.Specifications;

public class LengthMustBeValid(float length) : ISpecification
{
    public Result IsSatisfied()
    {
        return length < RoomConstants.MinLength 
                ? Result.Failure<float>(RoomErrors.WrongLengthValue)
                : Result.Success();
    }
}