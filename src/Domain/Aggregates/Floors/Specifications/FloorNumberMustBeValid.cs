using Core;
using Core.Results;

namespace Domain.Aggregates.Floors.Specifications;

public class FloorNumberMustBeValid(int floorNumber) : ISpecification
{
    public Result IsSatisfied()
    {
        return floorNumber < 1 
            ? Result.Failure<int>(FloorErrors.InvalidFloorCount) 
            : Result.Success();
    }
}