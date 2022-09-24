namespace Standards.Models.Departments
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int HousingId { get; set; }
        public int Floor { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int SectorId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
