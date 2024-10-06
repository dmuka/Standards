namespace Standards.Core.Models.Standards
{
    public class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double RangeStart { get; set; }
        public double RangeEnd { get; set; }
        public Unit Unit { get; set; }
        public Grade Grade { get; set; }
        public double GradeValue { get; set; }
        public double GradeValueStart { get; set; }
        public double GradeValueEnd { get; set; }
        public Standard Standard { get; set; }
        public string Comments { get; set; } = null!;
    }
}
