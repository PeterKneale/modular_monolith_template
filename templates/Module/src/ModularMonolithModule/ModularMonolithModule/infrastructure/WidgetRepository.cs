using Dapper;
using ModularMonolithModule.Application;
using ModularMonolithModule.Domain;
using Npgsql;

namespace ModularMonolithModule.infrastructure;

public class WidgetRepository(string connectionString) : IWidgetRepository
{
    public async Task<bool> Exists(Guid id)
    {
        const string sql = "SELECT count(1) FROM widgets WHERE id = @id";
        var command = new CommandDefinition(sql, new { id });
        await using var connection = new NpgsqlConnection(connectionString);
        return await connection.ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task<bool> Exists(string name)
    {
        const string sql = "SELECT count(1) FROM widgets WHERE name = @name";
        var command = new CommandDefinition(sql, new { name });
        await using var connection = new NpgsqlConnection(connectionString);
        return await connection.ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task Add(Widget widget)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        const string sql = "INSERT INTO widgets (id, name, price) VALUES (@Id, @Name, @Price)";
        var command = new CommandDefinition(sql, widget);
        await connection.ExecuteAsync(command);
    }
}