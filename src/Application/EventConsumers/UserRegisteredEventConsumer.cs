using Application.Abstractions.Kafka;
using Domain.Events.Integration;
using Microsoft.Extensions.Logging;

namespace Application.EventConsumers;

public class UserRegisteredEventConsumer
{
    public class UserRoleUpdatedConsumer(IEventConsumer eventConsumer, ILogger<UserRoleUpdatedConsumer> logger)
    {
        public Task StartConsuming(CancellationToken cancellationToken)
        {
            return eventConsumer.ConsumeAsync<UserRegisteredIntegrationEvent>(
                "user-registered",
                async (@event) =>
                {
                    logger.LogInformation("User with id {UserId} registered", @event.UserId);
                    
                    await Task.CompletedTask;
                },
                cancellationToken);
        }
    }
}