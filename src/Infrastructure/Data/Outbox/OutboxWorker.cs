using System.Text.Json;
using Application.Exceptions;
using Core;
using Infrastructure.Exceptions.Enum;
using Infrastructure.Options.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace Infrastructure.Data.Outbox;

public class OutboxWorker(
    IServiceProvider serviceProvider,
    IOptions<OutboxWorkerOptions> options,
    ILogger<OutboxWorker> logger) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(options.Value.WorkIterationsDelayInSeconds);
    private readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<Exception>()
        .WaitAndRetryAsync(
            retryCount: options.Value.RetryCount,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(options.Value.RetryDelayStartValueInSeconds, attempt)),
            onRetry: (exception, delay, attempt, _) =>
            {
                if (attempt == options.Value.RetryCount)
                {
                    logger.LogError("Last retry #{Attempt} due to {Message}. Waiting {Delay}...", attempt, exception.Message, delay);
                }
                
                logger.LogWarning("Retry #{Attempt} due to {Message}. Waiting {Delay}...", attempt, exception.Message, delay);
            });

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _retryPolicy.ExecuteAsync(async () => 
                {
                    await ProcessOutboxMessagesAsync(cancellationToken);
                });
            }
            catch (Exception ex)
            {
                logger.LogWarning("Outbox processing failed permanently: {Message}", ex.Message);
            }

            await Task.Delay(_interval, cancellationToken);
        }
    }

    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        logger.LogInformation("Start process outbox messages.");
        
        var messages = await dbContext.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(options.Value.BatchSize)
            .ToListAsync(cancellationToken);

        logger.LogInformation("Get {Count} outbox messages from db.", messages.Count);
        
        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType($"Namespace.Events.{message.Type}");

                if (JsonSerializer.Deserialize(message.Content, type!) is not IDomainEvent domainEvent)
                    throw new StandardsException(
                        StatusCodeByError.InternalServerError, 
                        $"Event with id: {message.Id}, type: {message.Type}, occured at: {message.OccurredAt} have null content.", 
                        "InternalServerError");
                
                await mediator.Publish(domainEvent, cancellationToken);

                message.ProcessedAt = DateTime.UtcNow;
                message.Error = null;
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("End process outbox messages.");
    }
}