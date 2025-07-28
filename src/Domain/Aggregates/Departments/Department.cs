using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Sectors;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Departments;

public class Department : NamedAggregateRoot<DepartmentId>, ICacheable
{
    protected Department() { }
    public int SectorsCount { get; private set; }
    public IReadOnlyCollection<SectorId> SectorIds => _sectorIds.AsReadOnly();
    private List<SectorId> _sectorIds = [];

    private Department(
        DepartmentId departmentId, 
        Name name,
        ShortName shortName,
        string? comments = null)
    {
        Id = departmentId;
        Name = name;
        ShortName = shortName;
        Comments = comments;
    }

    public static Result<Department> Create(
        string departmentName, 
        string departmentShortName,
        Guid? departmentId = null,
        string? comments = null)
    {
        var validationResults = ValidateDepartmentDetails(
            departmentName,
            departmentShortName);
        if (validationResults.Length != 0)
            return Result<Department>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var department = new Department(
            departmentId is null ? new DepartmentId(Guid.CreateVersion7()) : new DepartmentId(departmentId.Value), 
            Name.Create(departmentName).Value, 
            ShortName.Create(departmentShortName).Value,
            comments);
            
        return Result.Success(department);
    }
    
    public void AddSector(SectorId sectorId)
    {
        if (!_sectorIds.Contains(sectorId))
        {
            _sectorIds.Add(sectorId);
            SectorsCount++;
        }
    }
    
    public void AddSectors(IList<SectorId> sectorIds)
    {
        if (_sectorIds.Any(sectorIds.Contains)) return;
        
        _sectorIds.AddRange(sectorIds);
        SectorsCount += sectorIds.Count;
    }

    public static string GetCacheKey()
    {
        return Cache.Departments;
    }

    /// <summary>
    /// Validates department details.
    /// </summary>
    private static Result[] ValidateDepartmentDetails(string departmentName, string departmentShortName)
    {
        var validationResults = new []
        {
            new NameMustHaveValidLength(departmentName).IsSatisfied(),
            new ShortNameMustHaveValidLength(departmentShortName).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}