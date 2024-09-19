using Common.Configuration;
using Common.Integration;
using Common.Migrations;
using Npgmq;

namespace Host.Infrastructure.Integration;

public class QueueProcessor(IConfiguration configuration, EventPublisher publisher, ILogger<QueueProcessor> log)
{
    const string QueueName = "widgets_queue";
    
    public async Task ProcessQueueAsync(CancellationToken stoppingToken)
    {
        var connectionString = configuration.GetDbConnectionString("pgmq");
        var queue = new NpgmqClient(connectionString);
        await queue.InitAsync();
        while (!await queue.QueueExistsAsync(QueueName) && !stoppingToken.IsCancellationRequested)
        {
            log.LogInformation("Waiting for queue {Name} to be created", QueueName);
            await Task.Delay(1000, stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var envelope = await queue.ReadAsync<IntegrationEventEnvelope>(QueueName);
            if (envelope is not null)
            {
                log.LogInformation("Handling message {Message}", envelope.MsgId);
                await publisher.Publish(envelope.Message!);

                var success = await queue.ArchiveAsync(QueueName, envelope.MsgId);
                if (success)
                {
                    log.LogInformation("Archived message {Message}", envelope.MsgId);
                }
                else
                {
                    log.LogError("Failed to archive message {Message}", envelope.MsgId);
                }
            }
            else
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}