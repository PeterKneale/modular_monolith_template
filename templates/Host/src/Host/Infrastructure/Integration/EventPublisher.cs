using Common;
using Common.Integration;

namespace Host.Infrastructure.Integration;

public class EventPublisher(ILogger<EventPublisher> log, IEnumerable<IModule> modules)
{
    public async Task Publish(IntegrationEventEnvelope envelope)
    {
        var type = envelope.Type;
        var json = envelope.Json;
        log.LogInformation("Processing message {Type} {Json}", type, json);

        var message = envelope.GetMessage();

        if (modules.Any())
        {
            foreach(var module in modules)
            {
                log.LogInformation("Publishing message {Type} to {Module}", type, module.GetType().Name);
                await module.PublishNotification(message);
            }   
        }
        else
        {
            log.LogWarning("No modules found to publish message {Type}", type);
        }
    }
}