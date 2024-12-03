using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models.Standards;

public class Characteristic : Entity, ICacheable
{
    public double RangeStart { get; set; }
    public double RangeEnd { get; set; }
    public Unit Unit { get; set; }
    public Grade Grade { get; set; }
    public double GradeValue { get; set; }
    public double GradeValueStart { get; set; }
    public double GradeValueEnd { get; set; }
    public Standard Standard { get; set; }
        
    public static string GetCacheKey()
    {
        return Cache.Characteristics;
    }
}