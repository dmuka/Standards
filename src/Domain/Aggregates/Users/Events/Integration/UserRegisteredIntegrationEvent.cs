using Core;

namespace Domain.Aggregates.Users.Events.Integration;

public sealed record UserRegisteredIntegrationEvent(
    Guid UserId, 
    string FirstName, 
    string LastName, 
    string Email, 
    DateTime RegisteredAt) : IIntegrationEvent;