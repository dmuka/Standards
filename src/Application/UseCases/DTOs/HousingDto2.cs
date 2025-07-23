using Core;
using Domain.Aggregates.Common.ValueObjects;
using Domain.Aggregates.Housings;

namespace Application.UseCases.DTOs;

public class HousingDto2 : Entity
{
    public new required HousingId Id { get; set; }
    public required Name HousingName { get; set; }
    public required ShortName HousingShortName { get; set; }
    public required Address Address { get; set; }
    public IList<Guid> FloorIds { get; set; } = [];
    public IList<int> RoomIds { get; set; } = [];
    public string? Comments { get; set; }
}