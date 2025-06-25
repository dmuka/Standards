using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Rooms;
using Domain.Models;

namespace Application.UseCases.DTOs;

public class FloorDto : Entity
{
    public required FloorId FloorId { get; set; }
    public required HousingId HousingId { get; set; }
    public required int Number { get; set; }
    public IList<RoomId> RoomIds { get; set; } = [];
}