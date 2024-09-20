namespace Common.Infrastructure;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
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
}