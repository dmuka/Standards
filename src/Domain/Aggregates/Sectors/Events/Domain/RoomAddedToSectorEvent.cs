using Core;
using Domain.Aggregates.Rooms;

namespace Domain.Aggregates.Sectors.Events.Domain;

public sealed record RoomAddedToSectorEvent(SectorId SectorId, RoomId RoomId) : IDomainEvent
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
}