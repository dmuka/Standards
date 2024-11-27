namespace Standards.Core.Models.DTOs;

public class SectorDto : Entity
{
    public required int DepartmentId { get; set; }
    public IList<int> RoomIds { get; set; } = [];
    public IList<int> WorkplaceIds { get; set; } = [];
    public IList<int> PersonIds { get; set; } = [];
}