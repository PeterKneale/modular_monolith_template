using Dapper;
using ModularMonolithModule.Application;
using ModularMonolithModule.Domain;

namespace ModularMonolithModule.infrastructure;

public class WidgetRepository(IDbConnectionFactory connections) : IWidgetRepository
{
    public async Task<bool> Exists(Guid id)
    {
        const string sql = "SELECT count(1) FROM widgets WHERE id = @id";
        var command = new CommandDefinition(sql, new { id });
        return await connections.Create().ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task<bool> Exists(string name)
    {
        const string sql = "SELECT count(1) FROM widgets WHERE name = @name";
        var command = new CommandDefinition(sql, new { name });
        return await connections.Create().ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task Add(Widget widget)
    {
        const string sql = "INSERT INTO widgets (id, name, price) VALUES (@Id, @Name, @Price)";
        var command = new CommandDefinition(sql, widget);
        await connections.Create().ExecuteAsync(command);
    }
}