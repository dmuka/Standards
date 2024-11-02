using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;

namespace Standards.Core.Models.DTOs;

public class PersonDto
{
    public int Id { get; set; }
    [MaxLength(Lengths.PersonName)]
    public string FirstName { get; set; }
    [MaxLength(Lengths.PersonName)]
    public string MiddleName { get; set; }
    [MaxLength(Lengths.PersonName)]
    public string LastName { get; set; }
    public int CategoryId { get; set; }
    public int PositionId { get; set; }
    public DateTime BirthdayDate { get; set; }
    public int SectorId { get; set; }
    [MaxLength(Lengths.Role)]
    public string Role { get; set; }
    public int UserId { get; set; }
    [MaxLength(Lengths.Comment)]
    public string? Comments { get; set; } = null;
}