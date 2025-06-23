using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Interfaces;
using Domain.Models.Persons;
using Domain.Models.Services;

namespace Domain.Models.Standards;

public class Standard : Entity, ICacheable
{
    public required Person Responsible { get; set; }
    public required string? ImagePath { get; set; }
    public int VerificationInterval { get; set; }
    public int? CalibrationInterval { get; set; }
    public IList<Service> Services { get; set; } = [];
    public required IList<Workplace> Workplaces { get; set; } = [];
    public required IList<Characteristic> Characteristics { get; set; } = [];
        
    public static string GetCacheKey()
    {
        return Cache.Standards;
    }
}