using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Interfaces;
using Domain.Models.MetrologyControl.Contacts;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;

namespace Domain.Models.MetrologyControl;

public class Place : Entity, ICacheable
{
    public required string Address { get; set; }
    
    public IList<Contact> Contacts { get; set; } = [];

    public static PlaceDto ToDto(Place place)
    {
        return new PlaceDto
        {
            Id = place.Id,
            Name = place.Name,
            ShortName = place.ShortName,
            Address = place.Address,
            Comments = place.Comments,
            ContactIds = place.Contacts.Select(contact => contact.Id).ToList()
        };
    }

    public static Place ToEntity(
        PlaceDto placeDto,
        IList<Contact> contacts)
    {
        return new Place
        {
            Name = placeDto.Name,
            ShortName = placeDto.ShortName,
            Comments = placeDto.Comments,
            Address = placeDto.Address,
            Contacts = contacts
        };
    }
    
    public static string GetCacheKey()
    {
        return Cache.Places;
    }
}