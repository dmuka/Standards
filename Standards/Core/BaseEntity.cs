using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.Models.Interfaces;

namespace Standards.Core;

public abstract class BaseEntity : IEntity<int>
{
    public int Id { get; set; }
    [MaxLength(Lengths.EntityName)]
    public string Name { get; set; } = null!;
    [MaxLength(Lengths.ShortName)]
    public string ShortName { get; set; } = null!;
    [MaxLength(Lengths.Comment)]
    public string Comments { get; set; } = null!;
}