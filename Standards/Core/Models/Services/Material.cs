namespace Standards.Core.Models.Services
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Unit Unit { get; set; }
        public string Comments { get; set; } = null!;
    }
}
