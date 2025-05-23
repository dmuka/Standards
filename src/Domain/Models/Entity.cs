namespace Domain.Models;

public abstract class Entity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string? Comments { get; set; }
}