namespace Common.Infrastructure;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> OpenAsync();
}