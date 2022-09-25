namespace Standards.Models.Services
{
    public class ServicesJournal
    {
        public int Id { get; set; }
        public int StandardId { get; set; }
        public int PersonId { get; set; }
        public int ServiceId { get; set; }
        public DateOnly Date { get; set; }
        public string Comments { get; set; } = null!;
    }
}
