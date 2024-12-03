using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Interfaces;
using Domain.Models.Users;
using Standards.Core;
using Standards.Core.Constants;

namespace Domain.Models.Persons;

public class Person : BaseEntity, ICacheable
{
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
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    [MaxLength(Lengths.Comment)]
    public string Comments { get; set; } = null!;
    public static string GetCacheKey()
    {
        return Cache.Persons;
    }
}