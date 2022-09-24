namespace Standards.Models.Departments
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public IList<Room> Rooms { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
