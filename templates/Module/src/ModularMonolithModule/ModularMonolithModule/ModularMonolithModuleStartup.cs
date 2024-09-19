using System.Reflection;
using Common;
using Common.Configuration;
using Common.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularMonolithModule.Application;
using ModularMonolithModule.Infrastructure;
using Npgmq;

namespace ModularMonolithModule;

public class ModuleStartup(IConfiguration configuration, ILoggerProvider logs) : IModuleStartup
{
    public async Task InitializeAsync()
    {
        var connectionString = configuration.GetDbConnectionString(Schema);
        var assembly = Assembly.GetExecutingAssembly();

        var assemblies = new[]
        {
            ModularMonolithModuleAssemblyInfo.Assembly,
            CommonAssemblyInfo.Assembly
        };
        
        var provider = new ServiceCollection()
            .AddCommon()
            // application
            .AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies))
            .AddValidatorsFromAssemblies(assemblies)
            // infrastructure
            .AddScoped<IDbConnectionFactory,DbConnectionFactory>(c=>new DbConnectionFactory(connectionString))
            .AddScoped<IWidgetRepository, WidgetRepository>()
            // logging
            .AddLogging(c =>
            {
                c.AddProvider(logs);
            })
            // builder container
            .BuildServiceProvider();

        var c = new NpgmqClient(connectionString);
        await c.InitAsync();
        await c.CreateQueueAsync(QueueName);
        
        CompositionRoot.SetProvider(provider);

        DbMigrations.Apply(Schema, connectionString, assembly, reset: false);
        
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public Task DestroyAsync()
    {
        var connectionString = configuration.GetDbConnectionString(Schema);
        var assembly = Assembly.GetExecutingAssembly();

        DbMigrations.Apply(Schema, connectionString, assembly, reset: false);

        return Task.CompletedTask;
    }


}