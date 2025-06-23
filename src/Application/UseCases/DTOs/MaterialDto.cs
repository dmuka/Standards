using Domain.Models;

namespace Application.UseCases.DTOs;

public class MaterialDto : Entity
{
    public required int UnitId { get; set; }
}