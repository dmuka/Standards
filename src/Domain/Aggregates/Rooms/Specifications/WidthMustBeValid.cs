using Core;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms.Specifications;

public class WidthMustBeValid(float width) : ISpecification
{
    public Result IsSatisfied()
    {
        return width < RoomConstants.MinWidth 
                ? Result.Failure<float>(RoomErrors.WrongWidthValue)
                : Result.Success();
    }
}