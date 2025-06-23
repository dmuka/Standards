using Domain.Models;

namespace Application.UseCases.DTOs;

public class DepartmentDto : Entity
{
    public IList<int> SectorIds { get; set; } = [];
}