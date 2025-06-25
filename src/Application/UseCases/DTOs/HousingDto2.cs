using Domain.Aggregates.Housings;
using Domain.Models;

namespace Application.UseCases.DTOs;

public class HousingDto2 : Entity
{
    public required HousingId HousingId { get; set; }
    public required HousingName HousingName { get; set; }
    public required HousingShortName HousingShortName { get; set; }
    public required Address Address { get; set; }
    public string? Comments { get; set; }
    public int FloorsCount { get; set; }
    public IList<int> RoomIds { get; set; } = [];
}