using Domain.Models;

namespace Application.UseCases.DTOs;

public class SectorDto : Entity
{
    public int? DepartmentId { get; set; }
    public IList<int> RoomIds { get; set; } = [];
    public IList<int> WorkplaceIds { get; set; } = [];
    public IList<int> PersonIds { get; set; } = [];
}