namespace Standards.Models.Persons
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int CategoryId { get; set; }
        public int PositionId { get; set; }
        public DateOnly BirthdayDate { get; set; }
        public int DepartmentId { get; set; }
        public int SectorId { get; set; }
        public int RoleId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
