namespace Domain.Models;

public abstract class Entity : BaseEntity
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string? Comments { get; set; }
}