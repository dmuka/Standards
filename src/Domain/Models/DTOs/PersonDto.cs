using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;

namespace Domain.Models.DTOs;

public class PersonDto : Entity
{
    public int Id { get; set; }
    [MaxLength(Lengths.PersonName)]
    public required string FirstName { get; set; }
    [MaxLength(Lengths.PersonName)]
    public string? MiddleName { get; set; }
    [MaxLength(Lengths.PersonName)]
    public required string LastName { get; set; }
    public required int CategoryId { get; set; }
    public required int PositionId { get; set; }
    public required DateTime BirthdayDate { get; set; }
    public required int SectorId { get; set; }
    [MaxLength(Lengths.Role)]
    public required string Role { get; set; }
    public required int UserId { get; set; }
    [MaxLength(Lengths.Comment)]
    public string? Comments { get; set; }
}