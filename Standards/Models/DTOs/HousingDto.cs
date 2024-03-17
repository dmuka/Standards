namespace Standards.Models.DTOs
{
    public class HousingDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int FloorsCount { get; set; }
        public string Comments { get; set; } = null!;
    }
}