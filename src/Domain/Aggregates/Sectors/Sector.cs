using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors.Events.Domain;
using Domain.Aggregates.Workplaces;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Sectors;

public class Sector : NamedAggregateRoot<SectorId>, ICacheable
{
    protected Sector() { }

    public DepartmentId? DepartmentId { get; private set; }
    
    public IReadOnlyCollection<RoomId> RoomIds => _roomIds.AsReadOnly();
    private List<RoomId> _roomIds = [];
    public IList<WorkplaceId> WorkplaceIds => _workplaceIds.AsReadOnly();
    private List<WorkplaceId> _workplaceIds = [];
    public IList<PersonId> PersonIds => _personIds.AsReadOnly();
    private List<PersonId> _personIds = [];

    private Sector(
        Name name,
        ShortName shortName,
        SectorId sectorId, 
        DepartmentId? departmentId,
        string? comments)
    {
        Id = sectorId;
        Name = name;
        ShortName = shortName;
        DepartmentId = departmentId;
        Comments = comments;
    }

    public static Result<Sector> Create(
        string sectorName,
        string shortSectorName,
        Guid? sectorId = null,
        Guid? departmentId = null,
        string? comments = null)
    {
        var validationResults = ValidateSectorDetails(sectorName, shortSectorName);
        if (validationResults.Length != 0)
            return Result<Sector>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var sector = new Sector(
            Name.Create(sectorName).Value,
            ShortName.Create(shortSectorName).Value,
            sectorId is null ? new SectorId(Guid.CreateVersion7()) : new SectorId(sectorId.Value),
            departmentId is null ? null : new DepartmentId(departmentId.Value),
            comments);
            
        return Result.Success(sector);
    } 
    
    public Result AddPerson(PersonId personId)
    {
        if (_personIds.Contains(personId))
        {
            return Result.Failure(SectorErrors.PersonAlreadyExist);
        }
        
        _personIds.Add(personId);
        
        return Result.Success();
    }
    
    public Result RemovePerson(PersonId personId)
    {
        if (!_personIds.Contains(personId))
        {
            return Result.Failure(SectorErrors.PersonNotFound(personId));
        }
        
        _personIds.Remove(personId);
        
        return Result.Success();
    }
    
    public Result AddPersons(IList<PersonId> personIds)
    {
        if (_personIds.Any(personIds.Contains))
        {
            return Result.Failure(SectorErrors.OneOfThePersonAlreadyExist);
        }
        
        _personIds.AddRange(personIds);
        
        return Result.Success();
    }
    
    public Result AddWorkplace(WorkplaceId workplaceId)
    {
        if (_workplaceIds.Contains(workplaceId))
        {
            return Result.Failure(SectorErrors.WorkplaceAlreadyExist);
        }
        
        _workplaceIds.Add(workplaceId);
        
        return Result.Success();
    }
    
    public Result RemoveWorkplace(WorkplaceId workplaceId)
    {
        if (!_workplaceIds.Contains(workplaceId))
        {
            return Result.Failure(SectorErrors.WorkplaceNotFound(workplaceId));
        }
        
        _workplaceIds.Remove(workplaceId);
        
        return Result.Success();
    }
    
    public Result AddWorkplaces(IList<WorkplaceId> workplaceIds)
    {
        if (_workplaceIds.Any(workplaceIds.Contains))
        {
            return Result.Failure(SectorErrors.OneOfTheWorkplaceAlreadyExist);
        }
        
        _workplaceIds.AddRange(workplaceIds);
        
        return Result.Success();
    }
    
    public Result AddRoom(RoomId roomId)
    {
        if (_roomIds.Contains(roomId))
        {
            return Result.Failure(SectorErrors.RoomAlreadyExist);
        }
        
        _roomIds.Add(roomId);
        AddDomainEvent(new RoomAddedToSectorEvent(Id, roomId));
        
        return Result.Success();
    }
    
    public Result RemoveRoom(RoomId roomId)
    {
        if (!_roomIds.Contains(roomId))
        {
            return Result.Failure(SectorErrors.RoomNotFound(roomId));
        }
        
        _roomIds.Remove(roomId);
        
        return Result.Success();
    }
    
    public Result AddRooms(IList<RoomId> roomIds)
    {
        if (_roomIds.Any(roomIds.Contains))
        {
            return Result.Failure(SectorErrors.OneOfTheRoomAlreadyExist);
        }
        
        _roomIds.AddRange(roomIds);
        
        return Result.Success();
    }
    
    public Result ChangeDepartment(DepartmentId departmentId)
    {
        if (departmentId == DepartmentId) return Result.Failure(SectorErrors.ThisDepartmentAlreadySetForThisSector);
        
        DepartmentId = departmentId;
        
        return Result.Success();
    }
    
    public static string GetCacheKey()
    {
        return Cache.Sectors;
    }

    /// <summary>
    /// Validates sector details.
    /// </summary>
    private static Result[] ValidateSectorDetails(string sectorName, string shortSectorName)
    {
        var validationResults = new []
        {
            new NameMustHaveValidLength(sectorName).IsSatisfied(),
            new ShortNameMustHaveValidLength(shortSectorName).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}