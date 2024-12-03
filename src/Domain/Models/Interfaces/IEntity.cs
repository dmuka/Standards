namespace Domain.Models.Interfaces
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
        static abstract string GetCacheKey();
    }
}
