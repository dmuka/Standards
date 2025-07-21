using Core;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;

namespace Domain.Events.Domain;

public sealed record RoomAddedToSectorEvent(SectorId SectorId, RoomId RoomId) : IDomainEvent
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
}