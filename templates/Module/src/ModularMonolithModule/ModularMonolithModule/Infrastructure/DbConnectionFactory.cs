using ModularMonolithModule.Application;
using Npgsql;

namespace ModularMonolithModule.Infrastructure;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public NpgsqlConnection Create() => new(connectionString);
}