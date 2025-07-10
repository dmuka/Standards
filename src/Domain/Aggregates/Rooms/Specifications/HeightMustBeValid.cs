using Core;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms.Specifications;

public class HeightMustBeValid(float height) : ISpecification
{
    public Result IsSatisfied()
    {
        return height < RoomConstants.MinHeight 
                ? Result.Failure<float>(RoomErrors.WrongHeightValue)
                : Result.Success();
    }
}