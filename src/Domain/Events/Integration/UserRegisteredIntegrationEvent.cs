using Domain.Models.MetrologyControl.Contacts;

namespace Domain.Events.Integration;

public sealed record UserRegisteredIntegrationEvent(Guid UserId, Email Email, DateTime RegisteredAt) : IIntegrationEvent;