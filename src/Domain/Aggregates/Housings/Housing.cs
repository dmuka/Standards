using Core;
using Core.Results;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings.Specifications;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Housings;

public class Housing : AggregateRoot<HousingId>, ICacheable
{
    protected Housing() { }
    
    public HousingName HousingName { get; private set; } = null!;
    public HousingShortName HousingShortName { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public int FloorsCount { get; private set; }
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
        string housingName,
        string housingShortName, 
        string address,
        Guid? housingId = null,
        string? comments = null)
    {
        var validationResults = ValidateHousingDetails(housingName, housingShortName, address);
        if (validationResults.Length != 0)
            return Result<Housing>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var housing = new Housing(
            housingId is null ? new HousingId(Guid.CreateVersion7()) : new HousingId(housingId.Value), 
            HousingName.Create(housingName).Value, 
            HousingShortName.Create(housingShortName).Value, 
            Address.Create(address).Value, 
            comments);
            
        return Result.Success(housing);
    }    
    
    public Result Update(
        HousingName housingName,
        HousingShortName housingShortName,
        Address address,
        string? comments = null)
    {
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

    /// <summary>
    /// Validates housing details.
    /// </summary>
    private static Result[] ValidateHousingDetails(
        string housingName, 
        string housingShortName,
        string address)
    {
        var validationResults = new []
        {
            new HousingNameLengthMustBeValid(housingName).IsSatisfied(),
            new HousingShortNameLengthMustBeValid(housingShortName).IsSatisfied(),
            new AddressLengthMustBeValid(address).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}