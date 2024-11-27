namespace Standards.Core.Models.DTOs;

public class CharacteristicDto : Entity
{
    public required double RangeStart { get; set; }
    public required double RangeEnd { get; set; }
    public required int UnitId { get; set; }
    public required int GradeId { get; set; }
    public required double GradeValue { get; set; }
    public required double GradeValueStart { get; set; }
    public required double GradeValueEnd { get; set; }
    public int StandardId { get; set; }
}