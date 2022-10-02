using Standards.Models.Departments;

namespace Standards.Models.DTOs
{
    public class HousingDto
    {
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int FloorsCount { get; set; }
        public IList<Department> Departments { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}