﻿using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Services;

public class Material : Entity, ICacheable
{
    public Unit Unit { get; set; } = null!;

    public static string GetCacheKey()
    {
        return Cache.Materials;
    }
}