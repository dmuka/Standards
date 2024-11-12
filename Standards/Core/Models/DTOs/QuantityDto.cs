namespace Standards.Core.Models.DTOs;

public class QuantityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IList<int> UnitIds { get; set; } = null!;
}