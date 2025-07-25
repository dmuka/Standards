﻿using Core;
using Core.Results;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms.Specifications;

public class RoomHeightMustBeValid(float height) : ISpecification
{
    public Result IsSatisfied()
    {
        return height < RoomConstants.MinHeight 
                ? Result<float>.ValidationFailure(RoomErrors.WrongHeightValue)
                : Result.Success();
    }
}