using Core;
using Core.Results;

namespace Domain.Aggregates.Floors.Specifications;

public class FloorNumberMustBeGreaterThanZero(int floorNumber) : ISpecification
{
    public Result IsSatisfied()
    {
        return floorNumber < 1 
            ? Result<int>.ValidationFailure(FloorErrors.InvalidFloorCount) 
            : Result.Success();
    }
}