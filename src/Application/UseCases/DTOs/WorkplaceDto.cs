using Domain.Models;

namespace Application.UseCases.DTOs;

public class WorkplaceDto : Entity
{
    public required int RoomId { get; set; }
    public required int ResponsibleId { get; set; }
    public string? ImagePath { get; set; }
}