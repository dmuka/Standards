namespace Core;

public abstract class Entity<TId> where TId : TypedId
{
    public TId Id { get; set; }
}