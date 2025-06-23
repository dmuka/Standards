using Core;

namespace Domain.Aggregates.Housings.Specifications;

public class FloorCountMustBeValid(int floorCount) : ISpecification
{
    public Result IsSatisfied()
    {
        return floorCount < 1 
            ? Result.Failure<int>(HousingErrors.InvalidFloorCount) 
            : Result.Success();
    }
}