using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.Models;

namespace Standards.Core;

public abstract class BaseEntity : Entity
{
    [MaxLength(Lengths.EntityName)]
    public string Name { get; set; } = null!;
    [MaxLength(Lengths.ShortName)]
    public string ShortName { get; set; } = null!;
    [MaxLength(Lengths.Comment)]
    public string Comments { get; set; } = null!;
}