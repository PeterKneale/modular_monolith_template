using ModularMonolithModule.Application;
using Npgsql;

namespace ModularMonolithModule.infrastructure;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection Create() => new(connectionString);
}