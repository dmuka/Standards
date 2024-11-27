namespace Standards.Core.Models.Interfaces;

public interface ICacheable
{
    public static abstract string GetCacheKey();
}