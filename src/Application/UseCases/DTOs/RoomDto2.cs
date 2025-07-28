using Domain.Models;

namespace Application.UseCases.DTOs;

public class RoomDto2 : Entity
{
    public new required Guid Id { get; set; }
    public required Guid HousingId { get; set; }
    public required Guid FloorId { get; set; }
    public required float Length { get; set; }
    public required float Height { get; set; }
    public required float Width { get; set; }
    public IList<Guid> PersonIds { get; set; } = [];
    public IList<Guid> WorkplaceIds { get; set; } = [];
    public Guid? SectorId { get; set; }
}