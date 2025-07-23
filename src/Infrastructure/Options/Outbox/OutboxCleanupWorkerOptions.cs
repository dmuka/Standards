using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Options.Outbox;

public class OutboxCleanupWorkerOptions
{
    [Required, Range(24, 240)]
    public required int CleanupDelayInHours { get; set; }
    [Required, Range(1, 30)]
    public required int RetentionPeriodInDays { get; set; }
}