using Polly.Retry;

namespace Host.Infrastructure.Integration;

public class QueueProcessor
{
    private readonly QueueRepository _queue;
    private readonly EventPublisher _publisher;
    private readonly ILogger<QueueProcessor> _log;
    private readonly AsyncRetryPolicy _policy;

    public QueueProcessor(QueueRepository queue, EventPublisher publisher, ILogger<QueueProcessor> log)
    {
        _queue = queue;
        _publisher = publisher;
        _log = log;
        _policy = RetryHelper.BuildRetryPolicy(_log);
    }

    public async Task ProcessQueueAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("Queue processor started");

        // delay on start
        await Task.Delay(3000, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _policy.ExecuteAsync(async (cancellationToken) =>
            {
                await Process(cancellationToken);
            }, stoppingToken);
        }

        _log.LogInformation("Queue processor stopped");
    }

    private async Task Process(CancellationToken cancellationToken)
    {
        var envelope = await _queue.GetNextAsync(cancellationToken);

        if (envelope is null)
        {
            await Task.Delay(1000, cancellationToken);
            return;
        }

        try
        {
            await _policy.ExecuteAsync(async () =>
            {
                await _publisher.Publish(envelope, cancellationToken);
                await _queue.DeleteAsync(envelope.Id, cancellationToken);
            });
        }
        catch (Exception e)
        {
            _log.LogError(e, "Error processing message {Id}", envelope.Id);
        }
    }
}