using Domain.Models;

namespace Application.UseCases.DTOs;

public class ServiceJournalItemDto : Entity
{
    public required int StandardId { get; set; }
    public required int PersonId { get; set; }
    public required int ServiceId { get; set; }
    public required DateTime Date { get; set; }
}