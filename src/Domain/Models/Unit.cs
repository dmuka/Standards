using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Models;

public class Unit : BaseEntity, ICacheable
{
    public required Quantity Quantity { get; set; }
    public required string Name { get; set; }
    public required string Symbol { get; set; }
    public required string RuName { get; set; }
    public required string RuSymbol { get; set; }
        
    public static string GetCacheKey()
    {
        return Cache.Units;
    }
}