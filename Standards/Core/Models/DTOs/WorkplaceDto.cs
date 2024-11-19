namespace Standards.Core.Models.DTOs;

public class WorkplaceDto : BaseEntity
{
    public int RoomId { get; set; }
    public int ResponsibleId { get; set; }
    public string? ImagePath { get; set; }
}