using Core;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Workplaces;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Rooms;

public class Room : AggregateRoot, ICacheable
{
    protected Room() { }
    
    public Length Length { get; private set; }
    public Height Height { get; private set; }
    public Width Width { get; private set; }
    public FloorId FloorId { get; private set; }
    public SectorId? SectorId { get; private set; }
    public string? Comments { get; set; }
    
    public IReadOnlyCollection<WorkplaceId> WorkplaceIds => _workplaceIds.AsReadOnly();
    private List<WorkplaceId> _workplaceIds = [];
    public IReadOnlyCollection<PersonId> PersonIds => _personIds.AsReadOnly();
    private List<PersonId> _personIds = [];

    private Room(
        RoomId roomId, 
        Length length,
        Width width,  
        Height height, 
        string? comments = null)
    {
        Id = roomId;
        Length = length;
        Width = width;
        Height = height;
        Comments = comments;
    }

    public static Result<Room> Create(
        Length length, 
        Height height, 
        Width width,
        RoomId? roomId = null,
        string? comments = null)
    {
        var room = new Room(
            roomId ?? new RoomId(Guid.CreateVersion7()), 
            length, 
            width, 
            height, 
            comments);
            
        return Result.Success(room);
    }    
    
    public Result Update(
        Length length,
        Width width,
        Height height,
        string? comments = null)
         {
             if (!length.Equals(Length)) Length = length;
             if (!width.Equals(Width)) Width = width;
             if (!height.Equals(Height)) Height = height;
             if (comments != Comments) Comments = comments;
                 
             return Result.Success();
         }
    
    public Result AddPerson(PersonId personId)
    {
        if (_personIds.Contains(personId))
        {
            return Result.Failure(RoomErrors.PersonAlreadyExist);
        }
        
        _personIds.Add(personId);
        
        return Result.Success();
    }
    
    public Result AddPersons(IList<PersonId> personIds)
    {
        if (_personIds.Any(personIds.Contains))
        {
            return Result.Failure(RoomErrors.OneOfThePersonAlreadyExist);
        }
        
        _personIds.AddRange(personIds);
        
        return Result.Success();
    }
    
    public static string GetCacheKey()
    {
        return Cache.Housings;
    }
}