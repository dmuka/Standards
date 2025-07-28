using Core;
using Domain.Aggregates.Positions;

namespace Application.UseCases.DTOs;

public class PositionDto2 : Entity
{
    public new required PositionId Id { get; set; }
    public required string PositionName { get; set; }
    public required string PositionShortName { get; set; }
}