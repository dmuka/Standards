using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options.Kafka;

public record ConsumeOptions
{
    [Required, MinLength(5)]
    public required string BootstrapServers { get; set; }
    [Required, MinLength(5)]
    public required string TopicPrefix { get; set; }
    [Required, MinLength(5)]
    public required string ConsumerGroup { get; set; }
}