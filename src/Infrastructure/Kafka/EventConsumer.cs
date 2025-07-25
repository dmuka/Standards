using System.Text.Json;
using Application.Abstractions.Messaging;
using Confluent.Kafka;
using Infrastructure.Options.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka;

public class EventConsumer : IHostedService, IEventConsumer, IDisposable
{
    private readonly ILogger<EventConsumer> _logger;
    private readonly IOptions<ConsumeOptions> _consumeOptions;
    private IConsumer<Null, string>? _consumer;
    private readonly List<Task> _consumptionTasks = [];
    private readonly CancellationTokenSource _cts = new();

    public EventConsumer(
        IOptions<ConsumeOptions> consumeOptions,
        ILogger<EventConsumer> logger)
    {
        _consumeOptions = consumeOptions;
        _logger = logger;
    }

    public async Task ConsumeAsync<T>(
        string topic, 
        Func<T, Task> handler, 
        CancellationToken cancellationToken)
    {
        var fullTopic = $"{_consumeOptions.Value.TopicPrefix}{topic}";
        
        if (_consumer == null)
        {
            await InitializeConsumerAsync();
        }

        var consumptionTask = Task.Run(async () => 
        {
            _consumer!.Subscribe(fullTopic);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);
                    var @event = JsonSerializer.Deserialize<T>(result.Message.Value);
                    
                    if (@event is not null) 
                    {
                        await handler(@event);
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "Error consuming message from topic {Topic}: {Message}", fullTopic, e.Message);
                }
                catch (JsonException e)
                {
                    _logger.LogError(e, "Error deserializing message from topic {Topic}: {Message}", fullTopic, e.Message);
                }
                catch (OperationCanceledException)
                {
                    // Expected during shutdown
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unexpected error consuming from topic {Topic}: {Message}", fullTopic, e.Message);
                    await Task.Delay(5000, cancellationToken);
                }
            }
        }, cancellationToken);

        _consumptionTasks.Add(consumptionTask);
    }

    private Task InitializeConsumerAsync()
    {
        try
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _consumeOptions.Value.BootstrapServers,
                GroupId = _consumeOptions.Value.ConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<Null, string>(config)
                .SetErrorHandler((_, e) => 
                    _logger.LogError("Kafka error: {Reason}", e.Reason))
                .SetLogHandler((_, log) => 
                    _logger.Log(GetLogLevel(log.Level), "Kafka log: {Message}", log.Message))
                .Build();
            
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Kafka consumer");
            throw;
        }
    }

    private static LogLevel GetLogLevel(SyslogLevel level) => level switch
    {
        SyslogLevel.Emergency or SyslogLevel.Alert or SyslogLevel.Critical or SyslogLevel.Error => LogLevel.Error,
        SyslogLevel.Warning => LogLevel.Warning,
        SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
        SyslogLevel.Debug => LogLevel.Debug,
        _ => LogLevel.None
    };

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _cts.CancelAsync();
        
        try
        {
            if (_consumptionTasks.Count != 0)
            {
                await Task.WhenAll(_consumptionTasks);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while stopping consumption tasks");
        }
        finally
        {
            _consumer?.Close();
            _consumer?.Dispose();
        }
    }

    public void Dispose()
    {
        _cts.Dispose();
        _consumer?.Dispose();
        GC.SuppressFinalize(this);
    }
}