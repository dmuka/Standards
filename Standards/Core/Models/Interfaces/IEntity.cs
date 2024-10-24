namespace Standards.Core.Models.Interfaces
{
    public interface IEntity<out TId>
    {
        static abstract string GetCacheKey();
    }
}
