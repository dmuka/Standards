namespace Domain.Models.DTOs;

public class RoomDto : Entity
{
    public required int HousingId { get; set; }
    public required int Floor { get; set; }
    public required double Length { get; set; }
    public required double Height { get; set; }
    public required double Width { get; set; }
    public IList<int> PersonIds { get; set; } = [];
    public IList<int> WorkplaceIds { get; set; } = [];
    public required int SectorId { get; set; }
}