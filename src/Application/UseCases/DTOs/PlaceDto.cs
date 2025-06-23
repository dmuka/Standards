using Domain.Models;
using Domain.Models.MetrologyControl;
using Domain.Models.MetrologyControl.Contacts;

namespace Application.UseCases.DTOs;

public class PlaceDto : Entity
{
    public required string Address { get; set; }
    
    public IList<int> ContactIds { get; set; } = [];

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

}