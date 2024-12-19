namespace Domain.Models.DTOs;

public class StandardDto : Entity
{
    public required int ResponsibleId { get; set; }
    public required string? ImagePath { get; set; }
    public int VerificationInterval { get; set; }
    public int? CalibrationInterval { get; set; }
    public IList<int> ServiceIds { get; set; } = [];
    public required IList<int> WorkplaceIds { get; set; } = [];
    public required IList<int> CharacteristicIds { get; set; } = [];
}