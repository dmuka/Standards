namespace Domain.Models.Interfaces;

public interface ICacheable
{
    public static abstract string GetCacheKey();
}