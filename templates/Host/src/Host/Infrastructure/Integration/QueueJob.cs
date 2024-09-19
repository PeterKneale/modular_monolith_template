namespace Host.Infrastructure.Integration;

public class QueueJob(QueueProcessor processor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await processor.ProcessQueueAsync(stoppingToken);
    }
}