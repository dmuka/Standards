using Standards.Core.CQRS.Common.Constants;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Interfaces;
using Standards.Core.Models.Persons;
using Standards.Core.Models.Services;

namespace Standards.Core.Models.Standards;

public class Standard : Entity, ICacheable
{
    public required Person? Responsible { get; set; }
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