using Dapper;
using Microsoft.Extensions.Logging;
using static Common.Infrastructure.Integration.DbConstants;

namespace Common.Infrastructure.Integration;

public class IntegrationEventRepository(IDbConnectionFactory factory, ILogger<IntegrationEventRepository> log)
{
    public async Task SaveAsync<T>(T @event, CancellationToken token = default) where T : IntegrationEvent
    {
        var envelope = IntegrationEventEnvelope.Create(@event);

        log.LogInformation("Saving integration event {Id} of type {Type}", envelope.Id, envelope.Type);

        const string sql = $"INSERT INTO {Schema}.{IntegrationEventsTable} " +
                           $"({IdColumn},{TypeColumn},{JsonColumn}) " +
                           $"VALUES (@id, @type, @json)";
        var connection = await factory.OpenAsync();
        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            id = Guid.NewGuid(),
            type = envelope.Type,
            json = envelope.Json,
        }, cancellationToken: token));
    }
}