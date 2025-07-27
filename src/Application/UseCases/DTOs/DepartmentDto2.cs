using Core;
using Domain.Aggregates.Departments;

namespace Application.UseCases.DTOs;

public class DepartmentDto2 : Entity
{
    public new required DepartmentId Id { get; set; }
    public required string DepartmentName { get; set; }
    public required string DepartmentShortName { get; set; }
    public IList<int> SectorIds { get; set; } = [];
}