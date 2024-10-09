using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.DTOs;

public class RoomDto : BaseEntity
{
    public int HousingId { get; set; }
    public int Floor { get; set; }
    public double Length { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public IList<int> PersonIds { get; set; }
    public IList<int> WorkplaceIds { get; set; }
    public int SectorId { get; set; }
}