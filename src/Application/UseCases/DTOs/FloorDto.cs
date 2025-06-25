using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Rooms;

namespace Application.UseCases.DTOs;

public class FloorDto
{
    public required FloorId FloorId { get; set; }
    public required HousingId HousingId { get; set; }
    public required int Number { get; set; }
    public IList<RoomId> RoomIds { get; set; } = [];
}