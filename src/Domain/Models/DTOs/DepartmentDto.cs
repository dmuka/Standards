namespace Domain.Models.DTOs;

public class DepartmentDto : Entity
{
    public IList<int> SectorIds { get; set; } = [];
}