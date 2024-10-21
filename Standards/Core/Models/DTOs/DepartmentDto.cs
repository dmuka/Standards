namespace Standards.Core.Models.DTOs;

public class DepartmentDto : BaseEntity
{
    public IList<int> SectorIds { get; set; } = new List<int>();
    public IList<int> HousingIds { get; set; } = new List<int>();
}