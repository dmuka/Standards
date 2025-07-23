using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Floors;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Housings;

public class Housing : NamedAggregateRoot<HousingId>, ICacheable
{
    protected Housing() { }
    public Address Address { get; private set; } = null!;
    public int FloorsCount { get; private set; }
    public IReadOnlyCollection<FloorId> FloorIds => _floorIds.AsReadOnly();
    private List<FloorId> _floorIds = [];

    private Housing(
        HousingId housingId, 
        Name housingName, 
        ShortName housingShortName, 
        Address address, 
        string? comments = null)
    {
        Id = housingId;
        Address = address;
        Name = housingName;
        ShortName = housingShortName;
        Comments = comments;
    }

    public static Result<Housing> Create(
        Name housingName,
        ShortName housingShortName, 
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
        Name housingName,
        ShortName housingShortName,
        Address address,
        string? comments = null)
         {
             if (housingName is null) return Result<Housing>.ValidationFailure(HousingErrors.EmptyHousingName);
             if (!housingName.Equals(Name)) Name = housingName;
             if (!housingShortName.Equals(ShortName)) ShortName = housingShortName;
             if (!address.Equals(Address)) Address = address;
             if (comments != Comments) Comments = comments;
                 
             return Result.Success();
         }
    
    public void AddFloor(FloorId floorId)
    {
        if (!_floorIds.Contains(floorId))
        {
            _floorIds.Add(floorId);
            FloorsCount++;
        }
    }
    
    public void AddFloors(IList<FloorId> floorIds)
    {
        if (_floorIds.Any(floorIds.Contains)) return;
        
        _floorIds.AddRange(floorIds);
        FloorsCount += floorIds.Count;
    }
    
    public static string GetCacheKey()
    {
        return Cache.Housings;
    }
}