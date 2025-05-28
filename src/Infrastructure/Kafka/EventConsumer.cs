using System.Text.Json;
using Application.Abstractions.Kafka;
using Confluent.Kafka;
using Infrastructure.Options.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Kafka;

public class EventConsumer(IOptions<ConsumeOptions> consumeOptions) : IHostedService, IEventConsumer
{
    private readonly IConsumer<Null, string> _consumer = new ConsumerBuilder<Null, string>(
        new ConsumerConfig
        {
            BootstrapServers = consumeOptions.Value.BootstrapServers,
            GroupId = consumeOptions.Value.ConsumerGroup,
            AutoOffsetReset = AutoOffsetReset.Earliest
        }).Build();
    private readonly string _topicPrefix = consumeOptions.Value.TopicPrefix;

    public async Task ConsumeAsync<T>(string topic, Func<T, Task> handler, CancellationToken cancellationToken)
    {
        _consumer.Subscribe($"{_topicPrefix}{topic}");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);
                var @event = JsonSerializer.Deserialize<T>(result.Message.Value);
                
                if (@event is not null) await handler(@event);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _consumer.Close();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();
        
        return Task.CompletedTask;
    }
}