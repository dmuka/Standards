using Domain.Events.Integration;
using Infrastructure.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.EventConsumers;

public class UserRegisteredEventConsumer(
    IEventConsumer eventConsumer,
    ILogger<UserRegisteredEventConsumer> logger) : IHostedService
{
    private Task? _consumingTask;
    private CancellationTokenSource _cts = new();
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UserRegisteredEventConsumer");
        _consumingTask = StartConsuming(_cts.Token);
        return Task.CompletedTask;
    }

    private Task StartConsuming(CancellationToken cancellationToken)
    {
        return eventConsumer.ConsumeAsync<UserRegisteredIntegrationEvent>(
            "user-registered",
            async (@event) =>
            {
                logger.LogInformation("User with id {UserId} registered, Email={Email}", @event.UserId, @event.Email);
                
                await Task.CompletedTask;
            },
            cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping UserRegisteredEventConsumer");
        await _cts.CancelAsync();
        if (_consumingTask != null)
        {
            await Task.WhenAny(_consumingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
        _cts.Dispose();
    }
}