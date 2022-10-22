using Standards.Models.Departments;

namespace Standards.Models.Housings
{
    public class Housing
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int FloorsCount { get; set; }
        public IList<Department> Departments { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
