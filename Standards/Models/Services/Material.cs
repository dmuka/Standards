namespace Standards.Models.Services
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UnitId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
