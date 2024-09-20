using Common.Infrastructure.Configuration;
using Common.Infrastructure.Integration;
using Dapper;
using Npgsql;
using static Common.Infrastructure.Integration.DbConstants;

namespace Host.Infrastructure.Integration;

public class QueueRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetDbConnectionString(Schema);
    
    public async Task<IntegrationEventEnvelope?> GetNextAsync(CancellationToken token = default)
    {
        const string sql = $"SELECT * FROM {IntegrationEventsTable} LIMIT 1";
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<IntegrationEventEnvelope>(sql);
    }

    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        const string sql = $"DELETE FROM {IntegrationEventsTable} WHERE {IdColumn} = @id";
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(new CommandDefinition(sql, new { id }, cancellationToken: token));
    }
}