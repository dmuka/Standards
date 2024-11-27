namespace Standards.Core.Models.DTOs;

public class QuantityDto : Entity
{
    public IList<int> UnitIds { get; set; } = [];
}