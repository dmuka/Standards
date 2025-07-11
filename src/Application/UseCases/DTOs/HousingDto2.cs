﻿using Domain.Aggregates.Housings;

namespace Application.UseCases.DTOs;

public class HousingDto2
{
    public required HousingId HousingId { get; set; }
    public required HousingName HousingName { get; set; }
    public required HousingShortName HousingShortName { get; set; }
    public required Address Address { get; set; }
    public IList<Guid> FloorIds { get; set; } = [];
    public IList<int> RoomIds { get; set; } = [];
    public string? Comments { get; set; }
}