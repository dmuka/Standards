using Domain.Aggregates.Sectors;
using Domain.Models;

namespace Application.UseCases.DTOs;

public class SectorDto2 : Entity
{
    public new required SectorId Id { get; set; }
    public required string SectorName { get; set; }
    public required string SectorShortName { get; set; }
    public Guid? DepartmentId { get; set; }
    public IList<Guid> RoomIds { get; set; } = [];
    public IList<Guid> WorkplaceIds { get; set; } = [];
    public IList<Guid> PersonIds { get; set; } = [];
}