using Standards.Core.Models.Departments;
using Standards.Core.Models.Users;

namespace Standards.Core.Models.Persons
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Position Position { get; set; } = null!;
        public DateTime BirthdayDate { get; set; }
        public Department Department { get; set; } = null!;
        public Sector Sector { get; set; } = null!;
        public string Role { get; set; } = null!;
        public User User { get; set; } = null!;
        public string Comments { get; set; } = null!;
    }
}
