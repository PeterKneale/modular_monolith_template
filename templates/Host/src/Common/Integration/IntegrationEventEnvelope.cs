using MediatR;
using Newtonsoft.Json;

namespace Common.Integration;

public class IntegrationEventEnvelope(string type, string json)
{
    public string Type { get; init; } = type;
    public string Json { get; init; } = json;

    public static IntegrationEventEnvelope Create<T>(T o) where T : IntegrationEvent
    {
        var name = typeof(T).AssemblyQualifiedName;
        if (name == null)
        {
            throw new Exception($"Cannot get full name for type {typeof(T).Name}");
        }
        var json = JsonConvert.SerializeObject(o);
        return new IntegrationEventEnvelope(name, json);
    }

    public INotification GetMessage()
    {
        var type = System.Type.GetType(Type);
        if (type == null)
        {
            throw new Exception($"Cannot get type for {Type}");
        }

        if (JsonConvert.DeserializeObject(Json, type) is not INotification notification)
        {
            throw new Exception($"Cannot deserialize {Json} to {Type}");
        }
        return notification;
    }
}