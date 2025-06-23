using Domain.Models;

namespace Application.UseCases.DTOs;

public class ServiceDto : Entity
{
    public required int ServiceTypeId { get; set; }
    public IList<int> MaterialIds { get; set; } = [];
    public IList<int> MaterialsQuantityIds { get; set; } = [];
}