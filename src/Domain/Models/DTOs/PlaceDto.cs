namespace Domain.Models.DTOs;

public class PlaceDto : Entity
{
    public required string Address { get; set; }
    
    public IList<int> ContactIds { get; set; } = [];
}