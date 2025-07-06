using Core;
using Domain.Aggregates.Floors;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Housings;

public class Housing : AggregateRoot, ICacheable
{
    protected Housing() { }
    public Address Address { get; private set; }
    public HousingName HousingName { get; private set; }
    public HousingShortName? HousingShortName { get; private set; }
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
        if (housingName is null) return Result<Housing>.ValidationFailure(HousingErrors.EmptyHousingName);
        
        var housing = new Housing(
            housingId ?? new HousingId(Guid.CreateVersion7()), 
            housingName, 
            housingShortName, 
            address, 
            comments);
            
        return Result.Success(housing);
    }    
    
    public Result Update(
        HousingName housingName,
        HousingShortName housingShortName,
        Address address,
        string? comments = null)
         {
             if (housingName is null) return Result<Housing>.ValidationFailure(HousingErrors.EmptyHousingName);
             if (!housingName.Equals(HousingName)) HousingName = housingName;
             if (!housingShortName.Equals(HousingShortName)) HousingShortName = housingShortName;
             if (!address.Equals(Address)) Address = address;
             if (comments != Comments) Comments = comments;
                 
             return Result.Success();
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