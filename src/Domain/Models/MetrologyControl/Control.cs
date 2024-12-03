using Domain.Models.Standards;

namespace Domain.Models.MetrologyControl;

public abstract class Control
{
    public int Id { get; set; }
    public Standard Standard { get; set; }
    public Place Place { get; set; }
    public DateTime Date { get; set; }
    public DateTime ValidTo { get; set; }
    public string SertificateId { get; set; } = null!;
    public string SertificateImage { get; set; } = null!;
    public string Comments { get; set; } = null!;
}