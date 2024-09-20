using Microsoft.Extensions.Configuration;

namespace Common.Infrastructure.Configuration;

public static class DbConfigurationExtensions
{
    public static string GetDbConnectionString(this IConfiguration configuration, string schema)
    {
        var username = configuration["DB_USERNAME"] ?? "admin";
        var password = configuration["DB_PASSWORD"] ?? "password";
        var database = configuration["DB_DATABASE"] ?? "db";
        var host = configuration["DB_HOST"] ?? "localhost";
        var port = configuration["DB_PORT"] ?? "5432";
        var connectionString = $"Username={username};Password={password};Database={database};Host={host};Port={port};Search Path={schema};Include Error Detail=true;Log Parameters=true";
        return connectionString;
    }
}