using Core;
using Core.Results;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Workplaces.Specifications;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Workplaces;

public class Workplace : AggregateRoot<WorkplaceId>, ICacheable
{
    protected Workplace() { }

    public RoomId RoomId { get; private set; } = null!;
    public PersonId ResponsibleId { get; private set; } = null!;
    public SectorId SectorId { get; private set; } = null!;
    public ImagePath? ImagePath { get; private set; }
    public IReadOnlyCollection<PersonId> PersonIds => _personIds.AsReadOnly();
    private readonly List<PersonId> _personIds = [];

    private Workplace(
        WorkplaceId workplaceId, 
        RoomId roomId,
        PersonId responsibleId,
        SectorId sectorId,
        ImagePath? imagePath = null, 
        string? comments = null)
    {
        Id = workplaceId;
        RoomId = roomId;
        ResponsibleId = responsibleId;
        SectorId = sectorId;
        ImagePath = imagePath;
        Comments = comments;
    }

    public static Result<Workplace> Create( 
        Guid roomId,
        Guid responsibleId,
        Guid sectorId,
        Guid? workplaceId = null,
        string? imagePath = null, 
        string? comments = null)
    {
        if (imagePath is not null)
        {
            var validationResults = ValidateWorkplaceDetails(imagePath);
            if (validationResults.Length != 0)
                return Result<Workplace>.ValidationFailure(ValidationError.FromResults(validationResults));
        }

        var workplace = new Workplace(
            workplaceId is null ? new WorkplaceId(Guid.CreateVersion7()) : new WorkplaceId(workplaceId.Value), 
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
    private static Result[] ValidateWorkplaceDetails(string imagePath)
    {
        var validationResults = new []
        {
            new ImagePathMustBeValid(imagePath).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}