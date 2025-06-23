namespace Core;

public abstract class Entity<TId> where TId : TypedId
{
    public TId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string? Comments { get; set; }
}