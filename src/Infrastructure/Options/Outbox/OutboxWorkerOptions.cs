using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options.Outbox;

public class OutboxWorkerOptions
{
    [Required, Range(20, 200)]
    public required int BatchSize { get; set; }
    [Required, Range(5, 3600)]
    public required int WorkIterationsDelayInSeconds { get; set; }
    [Required, Range(3, 10)]
    public required int RetryCount { get; set; }
    [Required, Range(3, 10)]
    public required int RetryDelayStartValueInSeconds { get; set; }
}