using Domain.Models;

namespace Application.UseCases.DTOs;

public class HousingDto : Entity
{
    public required string Address { get; set; }
    public int FloorsCount { get; set; }
    public IList<int> RoomIds { get; set; } = [];
}