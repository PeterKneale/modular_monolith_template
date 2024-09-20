namespace Common.Infrastructure;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory, IDisposable, IAsyncDisposable
{
    private NpgsqlConnection? _connection;

    public async Task<NpgsqlConnection> OpenAsync()
    {
        if (_connection is not null)
        {
            return _connection;
        }
        _connection = new NpgsqlConnection(connectionString);
        await _connection.OpenAsync();
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null) await _connection.DisposeAsync();
    }
}