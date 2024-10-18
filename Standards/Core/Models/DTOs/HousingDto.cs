namespace Standards.Core.Models.DTOs;

public class HousingDto : BaseEntity
{
    public string Address { get; set; } = null!;
    public int FloorsCount { get; set; }
    public IList<int> DepartmentIds { get; set; }
    public IList<int> RoomIds { get; set; }
}