using Core;
using Domain.Models.MetrologyControl.Contacts;

namespace Domain.Aggregates.Users.Events.Integration;

public sealed record UserRegisteredIntegrationEvent(Guid UserId, string FirstName, string LastName, Email Email, DateTime RegisteredAt) : IIntegrationEvent;