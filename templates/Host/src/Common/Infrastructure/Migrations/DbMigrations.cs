using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Common.Infrastructure.Migrations;

public static class DbMigrations
{
    private const int RetryAttempts = 10;
    private static readonly TimeSpan RetryInterval = TimeSpan.FromSeconds(1);

    public static void Apply(string schema, string connectionString, Assembly assembly, bool reset = false)
    {
        using var provider = BuildServiceProvider(schema, connectionString, assembly);
        var logs = provider.GetRequiredService<ILogger<IMigrationRunner>>();
        var policy = BuildRetryPolicy(logs);
        policy.Execute(() =>
        {
            using var scope = provider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            logs.LogInformation("Migrating database schema");
            if (reset)
            {
                runner.MigrateDown(0);
            }

            runner.MigrateUp();
            logs.LogInformation("Migrated database schema");
        });
    }

    private static ServiceProvider BuildServiceProvider(string schema, string connectionString, Assembly assembly) =>
        new ServiceCollection()
            .AddSingleton<IConventionSet>(new DefaultConventionSet(schema, null))
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(assembly).For.Migrations())
            .BuildServiceProvider();

    private static RetryPolicy BuildRetryPolicy(ILogger<IMigrationRunner> logs) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetry(
                RetryAttempts,
                retryAttempt => RetryInterval,
                (exception, timeSpan, attempt, context) =>
                    logs.LogWarning($"Attempt {attempt} of {RetryAttempts} failed with exception {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
}