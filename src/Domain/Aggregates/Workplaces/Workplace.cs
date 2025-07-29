using Core.Results;
using Domain.Aggregates.Common;
using Domain.Aggregates.Common.Specifications;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Standards;
using Domain.Aggregates.Workplaces.Specifications;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Workplaces;

public class Workplace : NamedAggregateRoot<WorkplaceId>, ICacheable
{
    protected Workplace() { }

    public RoomId RoomId { get; private set; } = null!;
    public PersonId ResponsibleId { get; private set; } = null!;
    public SectorId SectorId { get; private set; } = null!;
    public ImagePath? ImagePath { get; private set; }
    public IReadOnlyCollection<PersonId> PersonIds => _personIds.AsReadOnly();
    private readonly List<PersonId> _personIds = [];
    public IReadOnlyCollection<StandardId> StandardIds => _standardIds.AsReadOnly();
    private readonly List<StandardId> _standardIds = [];

    private Workplace(
        WorkplaceId workplaceId,
        Name workplaceName,
        ShortName workplaceShortName,
        RoomId roomId,
        PersonId responsibleId,
        SectorId sectorId,
        ImagePath? imagePath = null, 
        string? comments = null)
    {
        Id = workplaceId;
        Name = workplaceName;
        ShortName = workplaceShortName;
        RoomId = roomId;
        ResponsibleId = responsibleId;
        SectorId = sectorId;
        ImagePath = imagePath;
        Comments = comments;
    }

    public static Result<Workplace> Create( 
        string workplaceName,
        string workplaceShortName,
        Guid roomId,
        Guid responsibleId,
        Guid sectorId,
        Guid? workplaceId = null,
        string? imagePath = null, 
        string? comments = null)
    {
        var validationResults = ValidateWorkplaceDetails(workplaceName, workplaceShortName, imagePath);
        if (validationResults.Length != 0)
            return Result<Workplace>.ValidationFailure(ValidationError.FromResults(validationResults));

        var workplace = new Workplace(
            workplaceId is null ? new WorkplaceId(Guid.CreateVersion7()) : new WorkplaceId(workplaceId.Value), 
            Name.Create(workplaceName).Value,
            ShortName.Create(workplaceShortName).Value,
            new RoomId(roomId),
            new PersonId(responsibleId), 
            new SectorId(sectorId), 
            imagePath is null ? null : ImagePath.Create(imagePath).Value, 
            comments);
            
        return Result.Success(workplace);
    }
    
    public Result AddPerson(PersonId personId)
    {
        if (_personIds.Contains(personId))
        {
            return Result.Failure(WorkplaceErrors.PersonAlreadyExist);
        }
        
        _personIds.Add(personId);
        
        return Result.Success();
    }
    
    public Result RemovePerson(PersonId personId)
    {
        if (!_personIds.Contains(personId))
        {
            return Result.Failure(WorkplaceErrors.PersonNotFound(personId));
        }
        
        _personIds.Remove(personId);
        
        return Result.Success();
    }
    
    public Result AddPersons(IList<PersonId> personIds)
    {
        if (_personIds.Any(personIds.Contains))
        {
            return Result.Failure(WorkplaceErrors.OneOfThePersonAlreadyExist);
        }
        
        _personIds.AddRange(personIds);
        
        return Result.Success();
    }
    
    public Result AddStandard(StandardId standardId)
    {
        if (_standardIds.Contains(standardId))
        {
            return Result.Failure(WorkplaceErrors.StandardAlreadyExist);
        }
        
        _standardIds.Add(standardId);
        
        return Result.Success();
    }
    
    public Result RemoveStandard(StandardId standardId)
    {
        if (!_standardIds.Contains(standardId))
        {
            return Result.Failure(WorkplaceErrors.StandardNotFound(standardId));
        }
        
        _standardIds.Remove(standardId);
        
        return Result.Success();
    }
    
    public Result AddStandards(IList<StandardId> standardIds)
    {
        if (_standardIds.Any(standardIds.Contains))
        {
            return Result.Failure(WorkplaceErrors.OneOfTheStandardAlreadyExist);
        }
        
        _standardIds.AddRange(standardIds);
        
        return Result.Success();
    }
    
    public Result ChangeSector(SectorId sectorId)
    {
        if (sectorId == SectorId) return Result.Failure(RoomErrors.ThisSectorAlreadySetForThisRoom);
        
        SectorId = sectorId;
        
        return Result.Success();
    }
    
    public Result ChangeImage(ImagePath imagePath)
    {
        if (imagePath == ImagePath) return Result.Failure(WorkplaceErrors.ThisImageAlreadySetForThisWorkplace(imagePath.Value));
        
        ImagePath = imagePath;
        
        return Result.Success();
    }
    
    public Result ChangeResponsible(PersonId responsibleId)
    {
        if (responsibleId == ResponsibleId) return Result.Failure(WorkplaceErrors.ThisPersonAlreadySetAsResponsibleForThisWorkplace(responsibleId));
        
        ResponsibleId = responsibleId;
        
        return Result.Success();
    }
    
    public static string GetCacheKey()
    {
        return Cache.Rooms;
    }

    /// <summary>
    /// Validates workplace details.
    /// </summary>
    private static Result[] ValidateWorkplaceDetails(
        string workplaceName, 
        string workplaceShortName, 
        string? imagePath)
    {
        var validationResults = new []
        {
            new NameMustHaveValidLength(workplaceName).IsSatisfied(),
            new ShortNameMustHaveValidLength(workplaceShortName).IsSatisfied()
        };
        
        if (imagePath is not null)
        {
            validationResults = validationResults.Append(new ImagePathMustBeValid(imagePath).IsSatisfied()).ToArray();
        }
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}