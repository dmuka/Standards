using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Workplaces;

namespace Domain.Aggregates.Departments;

public class Department : NamedAggregateRoot<DepartmentId>
{
    protected Department() { }
    
    public IReadOnlyCollection<WorkplaceId> WorkplaceIds => _workplaceIds.AsReadOnly();
    private List<WorkplaceId> _workplaceIds = [];
    public IReadOnlyCollection<PersonId> PersonIds => _personIds.AsReadOnly();
    private List<PersonId> _personIds = [];
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

    protected static Result<Department> Create(
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