using Domain.Models;

namespace Application.UseCases.DTOs;

public class QuantityDto : Entity
{
    public IList<int> UnitIds { get; set; } = [];
}