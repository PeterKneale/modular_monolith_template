using ModularMonolithModule.Application;
using Npgsql;

namespace ModularMonolithModule.Infrastructure;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection Create() => new(connectionString);

    public async Task<NpgsqlConnection> OpenAsync()
    {
        var connection = Create();
        await connection.OpenAsync();
        return connection;
    }
}