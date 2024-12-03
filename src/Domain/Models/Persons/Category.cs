using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Persons;

public class Category : Entity, ICacheable
{
    public static string GetCacheKey()
    {
        return Cache.Categories;
    }
}