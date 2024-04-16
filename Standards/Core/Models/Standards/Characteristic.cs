namespace Standards.Core.Models.Standards
{
    public class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double RangeStart { get; set; }
        public double RangeEnd { get; set; }
        public int UnitId { get; set; }
        public int GradeId { get; set; }
        public double GradeValue { get; set; }
        public double GradeValueStart { get; set; }
        public double GradeValueEnd { get; set; }
        public int StandardId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
