using Core;
using Domain.Models.MetrologyControl.Contacts;

namespace Domain.Aggregates.Persons.Events.Integration;

public sealed record UserRegisteredIntegrationEvent(Guid UserId, Email Email, DateTime RegisteredAt) : IIntegrationEvent;