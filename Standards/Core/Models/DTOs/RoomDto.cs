using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.DTOs;

public class RoomDto : BaseEntity
{
    public int HousingId { get; set; }
    public int Floor { get; set; }
    public int Length { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int SectorId { get; set; }
}