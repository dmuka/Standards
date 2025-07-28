using Core;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Housings;

namespace Application.UseCases.DTOs;

public class HousingDto2 : Entity
{
    public new required HousingId Id { get; set; }
    public required string HousingName { get; set; }
    public required string HousingShortName { get; set; }
    public required string Address { get; set; }
    public IList<Guid> FloorIds { get; set; } = [];
    public IList<Guid> RoomIds { get; set; } = [];
}