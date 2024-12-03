using System.ComponentModel.DataAnnotations;
using Standards.Core;
using Standards.Core.Constants;

namespace Domain.Models;

public abstract class Entity : BaseEntity
{
    [MaxLength(Lengths.EntityName)]
    public string Name { get; set; }
    [MaxLength(Lengths.ShortName)]
    public string ShortName { get; set; }
    [MaxLength(Lengths.Comment)]
    public string? Comments { get; set; }
}