using ModularMonolithModule.Application;

namespace ModularMonolithModule.Infrastructure;

public class WidgetRepository(IDbConnectionFactory connections) : IWidgetRepository
{
    public async Task<bool> Exists(Guid id)
    {
        const string sql = $"SELECT count(1) FROM {WidgetsTable} WHERE {IdColumn} = @id";
        var command = new CommandDefinition(sql, new { id });
        return await (await connections.OpenAsync()).ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task<bool> Exists(string name)
    {
        const string sql = $"SELECT count(1) FROM {WidgetsTable} WHERE {NameColumn} = @name";
        var command = new CommandDefinition(sql, new { name });
        var connection = await connections.OpenAsync();
        return await connection.ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task Add(Widget widget)
    {
        const string sql = $"INSERT INTO {WidgetsTable} ({IdColumn}, {NameColumn}, {PriceColumn}) VALUES (@Id, @Name, @Price)";
        var command = new CommandDefinition(sql, widget);
        var connection = await connections.OpenAsync();
        await connection.ExecuteAsync(command);
    }
}