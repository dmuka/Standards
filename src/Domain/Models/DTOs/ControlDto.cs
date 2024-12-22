namespace Domain.Models.DTOs;

public abstract class ControlDto : BaseEntity
{
    public required int StandardId { get; set; }
    public required int PlaceId { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime ValidTo { get; set; }
    public required string CertificateId { get; set; } 
    public string? CertificateImage { get; set; }
    public string? Comments { get; set; }
}