using Common.Integration;
using ModularMonolithModule.Application;
using ModularMonolithModule.Contracts;
using Npgmq;

namespace ModularMonolithModule.Infrastructure;

public class WidgetRepository(IDbConnectionFactory connections) : IWidgetRepository
{
    public async Task<bool> Exists(Guid id)
    {
        var sql = $"SELECT count(1) FROM {WidgetsTable} WHERE {IdColumn} = @id";
        var command = new CommandDefinition(sql, new { id });
        return await connections.Create().ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task<bool> Exists(string name)
    {
        var sql = $"SELECT count(1) FROM {WidgetsTable} WHERE {NameColumn} = @name";
        var command = new CommandDefinition(sql, new { name });
        return await connections.Create().ExecuteScalarAsync<int>(command) > 0;
    }

    public async Task Add(Widget widget)
    {
        var sql = $"INSERT INTO {WidgetsTable} ({IdColumn}, {NameColumn}, {PriceColumn}) VALUES (@Id, @Name, @Price)";
        var command = new CommandDefinition(sql, widget);
        var connection = await connections.OpenAsync();
        await using var tx = await connection.BeginTransactionAsync();
        await connection.ExecuteAsync(command);
        var queue = new NpgmqClient(connection);
        var evnt = new WidgetCreatedEvent { Id = widget.Id, Name = widget.Name, Price = widget.Price };
        var envelope = IntegrationEventEnvelope.Create(evnt);
        await queue.SendAsync(QueueName, envelope);
        await tx.CommitAsync();
    }
}