using Core;

namespace Application.UseCases.DTOs;

public class WorkplaceDto2 : Entity
{
    public new required Guid Id { get; set; }
    public required string WorkplaceName { get; set; }
    public required string WorkplaceShortName { get; set; }
    public required Guid RoomId { get; set; }
    public required Guid ResponsibleId { get; set; }
    public required Guid SectorId { get; set; }
    public string? ImagePath { get; set; }
    public IList<Guid> PersonIds { get; set; } = [];
    public IList<Guid> StandardIds { get; set; } = [];
}