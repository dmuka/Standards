namespace Standards.Core.Models.Departments
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public IList<Sector> Sectors { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
