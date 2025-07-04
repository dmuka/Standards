using Core;
using Domain.Aggregates.Floors;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Housings;

public class Housing : AggregateRoot, ICacheable
{
    protected Housing() { }
    public required HousingName HousingName { get; set; }
    public HousingShortName? HousingShortName { get; set; }
    public required Address Address { get; set; }
    public string? Comments { get; set; }
    public IReadOnlyCollection<FloorId> FloorIds => _floorIds.AsReadOnly();
    private List<FloorId> _floorIds = [];

    private Housing(
        HousingId housingId, 
        HousingName housingName, 
        HousingShortName housingShortName, 
        Address address, 
        string? comments = null)
    {
        Id = housingId;
        Address = address;
        HousingName = housingName;
        HousingShortName = housingShortName;
        Comments = comments;
    }

    public static Result<Housing> Create(
        HousingName housingName,
        HousingShortName housingShortName, 
        Address address,
        HousingId? housingId = null,
        string? comments = null)
    {
        var housing = new Housing(
            housingId ?? new HousingId(Guid.CreateVersion7()), 
            housingName, 
            housingShortName, 
            address, 
            comments)
        {
            HousingName = housingName,
            Address = address
        };
            
        return Result.Success(housing);
    }
    
    public void AddFloor(FloorId floorId)
    {
        if (!_floorIds.Contains(floorId))
        {
            _floorIds.Add(floorId);
        }
    }
    
    public void AddFloors(IList<FloorId> floorIds)
    {
        if (!_floorIds.Any(floorIds.Contains))
        {
            _floorIds.AddRange(floorIds);
        }
    }
    
    public static string GetCacheKey()
    {
        return Cache.Housings;
    }
}