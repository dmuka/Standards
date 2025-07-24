using Application.Abstractions.Messaging;
using Domain.Aggregates.Users.Events.Integration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.EventConsumers;

public class UserRegisteredEventConsumer(
    IEventConsumer eventConsumer,
    IServiceProvider serviceProvider,
    ILogger<UserRegisteredEventConsumer> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting UserRegisteredEventConsumer");
        
        try
        {
            await eventConsumer.ConsumeAsync<UserRegisteredIntegrationEvent>(
                "user-registered",
                async @event =>
                {
                    await using (var scope = serviceProvider.CreateAsyncScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        
                        await mediator.Publish(@event, cancellationToken);
                    }

                    logger.LogInformation(
                        "User with id {UserId} registered, Email={Email}", 
                        @event.UserId, @event.Email);
                    await Task.CompletedTask;
                },
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
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