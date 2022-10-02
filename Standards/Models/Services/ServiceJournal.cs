namespace Standards.Models.Services
{
    public class ServiceJournal
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int PersonId { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; } = null!;
    }
}
