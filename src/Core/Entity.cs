using System.ComponentModel.DataAnnotations;

namespace Core;

public abstract class Entity
{
    [Key]
    public TypedId Id { get; set; }
}