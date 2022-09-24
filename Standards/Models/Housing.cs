using Standards.Models.Departments;

namespace Standards.Models
{
    public class Housing
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public IList<Department> Departments { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
