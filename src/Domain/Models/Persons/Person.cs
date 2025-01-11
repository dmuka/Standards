using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Interfaces;
using Domain.Models.Users;

namespace Domain.Models.Persons;

public class Person : BaseEntity, ICacheable
{
    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Category? Category { get; set; } = null!;
    public Position? Position { get; set; } = null!;
    public DateTime BirthdayDate { get; set; }
    public Sector? Sector { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int UserId { get; set; }
    public User? User { get; set; } = null!;
    public string Comments { get; set; } = null!;
    public static string GetCacheKey()
    {
        return Cache.Persons;
    }
}