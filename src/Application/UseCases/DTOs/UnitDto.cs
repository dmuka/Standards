using Domain.Models;

namespace Application.UseCases.DTOs;

public class UnitDto : Entity
{
    public required int QuantityId { get; set; }
    public required string Symbol { get; set; }
    public required string RuName { get; set; }
    public required string RuSymbol { get; set; }
}