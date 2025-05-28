namespace Infrastructure.Kafka;

public interface IEventConsumer
{
    Task ConsumeAsync<T>(string topic, Func<T, Task> handler, CancellationToken cancellationToken);
}