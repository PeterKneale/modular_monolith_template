using Npgsql;

namespace ModularMonolithModule.Application;

public interface IDbConnectionFactory
{
    NpgsqlConnection Create();
    Task<NpgsqlConnection> OpenAsync();
}