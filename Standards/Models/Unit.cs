namespace Standards.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public int QuantityId { get; set; }
        public string Name { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string RuName { get; set; } = null!;
        public string RuSymbol { get; set; } = null!;
    }
}
