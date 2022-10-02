namespace Standards.Models.MetrologyControl
{
    public abstract class Control
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int PlaceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ValidTo { get; set; }
        public string SertificateId { get; set; } = null!;
        public string SertificateImage { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
