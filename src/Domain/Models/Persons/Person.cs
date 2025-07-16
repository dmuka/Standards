using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Interfaces;
using Domain.Models.Users;

namespace Domain.Models.Persons;

public class Person : BaseEntity, ICacheable
{
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public required string LastName { get; set; }
    public required Category Category { get; set; } 
    public required Position Position { get; set; }
    public DateTime BirthdayDate { get; set; }
    public required Sector Sector { get; set; }
    public required string Role { get; set; }
    public int UserId { get; set; }
    public required User User { get; set; }
    public string? Comments { get; set; }
    public static string GetCacheKey()
    {
        return Cache.Persons;
    }
}