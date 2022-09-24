namespace Standards.Models
{
    public class Quantity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IList<Unit> Units { get; set; } = null!;
    }
}
