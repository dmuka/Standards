using Core.Results;
using Domain.Aggregates.Common;
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
        Name name, 
        ShortName shortName,
        DepartmentId? departmentId = null,
        string? comments = null)
    {
        var department = new Department(
            departmentId ?? new DepartmentId(Guid.CreateVersion7()), 
            name, 
            shortName,
            comments);
            
        return Result.Success(department);
    }
}