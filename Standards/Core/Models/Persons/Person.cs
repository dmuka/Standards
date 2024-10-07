using System.ComponentModel.DataAnnotations;
using Standards.Core.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Users;

namespace Standards.Core.Models.Persons
{
    public class Person
    {
        public int Id { get; set; }
        [MaxLength(Lengths.PersonName)]
        public string FirstName { get; set; } = null!;
        [MaxLength(Lengths.PersonName)]
        public string MiddleName { get; set; } = null!;
        [MaxLength(Lengths.PersonName)]
        public string LastName { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public Position Position { get; set; } = null!;
        public DateTime BirthdayDate { get; set; }
        public Sector Sector { get; set; } = null!;
        [MaxLength(Lengths.Role)]
        public string Role { get; set; } = null!;
        public User User { get; set; } = null!;
        [MaxLength(Lengths.Comment)]
        public string Comments { get; set; } = null!;
    }
}
