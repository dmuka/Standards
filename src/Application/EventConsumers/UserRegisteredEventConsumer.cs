using Application.Abstractions.Messaging;
using Domain.Aggregates.Persons.Events.Integration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.EventConsumers;

public class UserRegisteredEventConsumer(
    IEventConsumer eventConsumer,
    ILogger<UserRegisteredEventConsumer> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting UserRegisteredEventConsumer");
        
        try
        {
            await eventConsumer.ConsumeAsync<UserRegisteredIntegrationEvent>(
                "user-registered",
                async (@event) =>
                {
                    logger.LogInformation(
                        "User with id {UserId} registered, Email={Email}", 
                        @event.UserId, @event.Email);
                    await Task.CompletedTask;
                },
                stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Stopped UserRegisteredEventConsumer");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in UserRegisteredEventConsumer");
            throw;
        }
    }
}