using Core;
using Core.Results;
using Domain.Aggregates.Categories;
using Domain.Aggregates.Persons.Specifications;
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

    public FirstName FirstName { get; private set; } = null!;
    public MiddleName MiddleName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public CategoryId CategoryId { get; private set; } = null!;
    public PositionId PositionId { get; private set; } = null!;
    public BirthdayDate BirthdayDate { get; private set; } = null!;
    public SectorId SectorId { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    
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
        string firstName,
        string middleName,  
        string lastName,
        DateOnly birthdayDate,
        Guid userId,
        Guid? personId = null,
        string? comments = null)
    {
        var validationResults = ValidatePersonDetails(
            firstName,
            middleName,
            lastName,
            birthdayDate);
        if (validationResults.Length != 0)
            return Result<Person>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var person = new Person(
            personId is null ? new PersonId(Guid.CreateVersion7()) : new PersonId(personId.Value), 
            FirstName.Create(firstName).Value, 
            MiddleName.Create(middleName).Value, 
            LastName.Create(lastName).Value,
            BirthdayDate.Create(birthdayDate).Value,
            new UserId(userId),
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

    /// <summary>
    /// Validates person details.
    /// </summary>
    private static Result[] ValidatePersonDetails(
        string firstName,
        string middleName,  
        string lastName,
        DateOnly birthdayDate)
    {
        var validationResults = new []
        {
            new FirstNameMustBeValid(firstName).IsSatisfied(),
            new MiddleNameMustBeValid(middleName).IsSatisfied(),
            new LastNameMustBeValid(lastName).IsSatisfied(),
            new BirthdayDateMustBeValid(birthdayDate).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}