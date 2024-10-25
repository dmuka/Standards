namespace Standards.Core.Models.DTOs;

public class CharacteristicDto : BaseEntity
{
    public double RangeStart { get; set; }
    public double RangeEnd { get; set; }
    public int UnitId { get; set; }
    public int GradeId { get; set; }
    public double GradeValue { get; set; }
    public double GradeValueStart { get; set; }
    public double GradeValueEnd { get; set; }
    public int StandardId { get; set; }
}