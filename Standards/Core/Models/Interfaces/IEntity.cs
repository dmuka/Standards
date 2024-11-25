namespace Standards.Core.Models.Interfaces
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
        static abstract string GetCacheKey();
    }
}
