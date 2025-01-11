using Domain.Models.Standards;

namespace Domain.Models.MetrologyControl;

public abstract class Control : BaseEntity
{
    public required Standard? Standard { get; set; }
    public required Place? Place { get; set; }
    public required DateTime Date { get; set; }
    public required DateTime ValidTo { get; set; }
    public required string CertificateId { get; set; } 
    public string? CertificateImage { get; set; }
    public string? Comments { get; set; }
}