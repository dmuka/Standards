using Microsoft.AspNetCore.Identity;

namespace Standards.Models.Persons
{
    public class Person : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int CategoryId { get; set; }
        public int PositionId { get; set; }
        public DateTime BirthdayDate { get; set; }
        public int DepartmentId { get; set; }
        public int SectorId { get; set; }
        public int RoleId { get; set; }
        public string Comments { get; set; } = null!;
    }
}
