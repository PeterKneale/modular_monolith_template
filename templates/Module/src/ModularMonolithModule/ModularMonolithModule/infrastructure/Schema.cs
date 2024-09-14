using Dapper;
using Npgsql;

namespace ModularMonolithModule.infrastructure;

public class Schema
{
    public static async Task CreateAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS widgets (
                id uuid PRIMARY KEY,
                name text NOT NULL,
                price numeric(18, 2) NOT NULL
            )");
    }

    public static async Task DropAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.ExecuteAsync("DROP TABLE IF EXISTS widgets");
    }
}