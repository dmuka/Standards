using Core;
using Core.Results;
using Domain.Aggregates.Categories;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Positions;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workplaces;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Persons;

public class Person : AggregateRoot<PersonId>, ICacheable
{
    protected Person() { }

    public FirstName FirstName { get; private set; }
    public MiddleName MiddleName { get; private set; }
    public LastName LastName { get; private set; }
    public CategoryId CategoryId { get; private set; } 
    public PositionId PositionId { get; private set; }
    public BirthdayDate BirthdayDate { get; private set; }
    public SectorId SectorId { get; private set; }
    public UserId UserId { get; private set; }
    
    public IReadOnlyCollection<WorkplaceId> WorkplaceIds => _workplaceIds.AsReadOnly();
    private List<WorkplaceId> _workplaceIds = [];

    private Person(
        PersonId roomId, 
        FirstName firstName,
        MiddleName middleName,  
        LastName lastName,
        BirthdayDate birthdayDate,
        UserId userId,
        string? comments = null)
    {
        Id = roomId;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthdayDate = birthdayDate;
        UserId = userId;
        Comments = comments;
    }

    public static Result<Person> Create(
        FirstName firstName,
        MiddleName middleName,  
        LastName lastName,
        BirthdayDate birthdayDate,
        UserId userId,
        PersonId? personId = null,
        string? comments = null)
    {
        var person = new Person(
            personId ?? new PersonId(Guid.CreateVersion7()), 
            firstName, 
            middleName, 
            lastName,
            birthdayDate,
            userId,
            comments);
            
        return Result.Success(person);
    }    
    
    public Result Update(
        FirstName firstName,
        MiddleName middleName,  
        LastName lastName,
        BirthdayDate birthdayDate,
        string? comments = null)
    {
        if (!firstName.Equals(FirstName)) FirstName = firstName;
        if (!middleName.Equals(MiddleName)) MiddleName = middleName;
        if (!lastName.Equals(LastName)) LastName = lastName;
        if (!birthdayDate.Equals(BirthdayDate)) BirthdayDate = birthdayDate;
        if (comments != Comments) Comments = comments;
        
        return Result.Success();
    }
    
    public Result ChangePosition(PositionId positionId)
    {
        if (positionId == PositionId) return Result.Failure(PersonErrors.ThisPositionAlreadySetForThisPerson);
        
        PositionId = positionId;
        
        return Result.Success();
    }
    
    public Result AddWorkplace(WorkplaceId workplaceId)
    {
        if (_workplaceIds.Contains(workplaceId))
        {
            return Result.Failure(PersonErrors.WorkplaceAlreadyExist);
        }
        
        _workplaceIds.Add(workplaceId);
        
        return Result.Success();
    }
    
    public Result RemoveWorkplace(WorkplaceId workplaceId)
    {
        if (!_workplaceIds.Contains(workplaceId))
        {
            return Result.Failure(PersonErrors.WorkplaceNotFound(workplaceId));
        }
        
        _workplaceIds.Remove(workplaceId);
        
        return Result.Success();
    }
    
    public Result AddWorkplaces(IList<WorkplaceId> workplaceIds)
    {
        if (_workplaceIds.Any(workplaceIds.Contains))
        {
            return Result.Failure(PersonErrors.OneOfTheWorkplaceAlreadyExist);
        }
        
        _workplaceIds.AddRange(workplaceIds);
        
        return Result.Success();
    }
    
    public Result ChangeSector(SectorId sectorId)
    {
        if (sectorId == SectorId) return Result.Failure(PersonErrors.ThisSectorAlreadySetForThisPerson);
        
        SectorId = sectorId;
        
        return Result.Success();
    }
    
    public static string GetCacheKey()
    {
        return Cache.Persons;
    }
}