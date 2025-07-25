﻿using Core;
using Core.Results;
using Domain.Aggregates.Rooms.Constants;

namespace Domain.Aggregates.Rooms.Specifications;

public class RoomLengthMustBeValid(float length) : ISpecification
{
    public Result IsSatisfied()
    {
        return length < RoomConstants.MinLength 
                ? Result<float>.ValidationFailure(RoomErrors.WrongLengthValue)
                : Result.Success();
    }
}