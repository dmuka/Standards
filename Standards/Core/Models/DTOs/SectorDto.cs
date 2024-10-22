namespace Standards.Core.Models.DTOs;

public class SectorDto : BaseEntity
{
    public int DepartmentId { get; set; }
    public IList<int> RoomIds { get; set; } = new List<int>();
    public IList<int> WorkplaceIds { get; set; } = new List<int>();
    public IList<int> PersonIds { get; set; } = new List<int>();
}