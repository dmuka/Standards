using Infrastructure.Options.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.Outbox;

public class OutboxCleanupWorker(
    IServiceProvider serviceProvider, 
    IOptions<OutboxCleanupWorkerOptions> options) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromHours(options.Value.CleanupDelayInHours);
    private readonly TimeSpan _retentionPeriod = TimeSpan.FromDays(options.Value.RetentionPeriodInDays);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                var cutoffDate = DateTime.UtcNow - _retentionPeriod;
                var oldMessages = await dbContext.OutboxMessages
                    .Where(m => m.ProcessedAt != null && m.ProcessedAt < cutoffDate)
                    .ToListAsync(cancellationToken);

                dbContext.OutboxMessages.RemoveRange(oldMessages);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            await Task.Delay(_interval, cancellationToken);
        }
    }
}