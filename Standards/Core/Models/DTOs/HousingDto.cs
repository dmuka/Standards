namespace Standards.Core.Models.DTOs;

public class HousingDto : Entity
{
    public required string Address { get; set; }
    public int FloorsCount { get; set; }
    public IList<int> RoomIds { get; set; } = [];
}