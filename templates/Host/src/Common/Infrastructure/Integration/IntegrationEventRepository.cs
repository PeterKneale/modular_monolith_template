using Dapper;
using static Common.Infrastructure.Integration.DbConstants;

namespace Common.Infrastructure.Integration;

public class IntegrationEventRepository(IDbConnectionFactory factory)
{
    public async Task SaveAsync<T>(T @event, CancellationToken token = default) where T : IntegrationEvent
    {
        var envelope = IntegrationEventEnvelope.Create(@event);

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