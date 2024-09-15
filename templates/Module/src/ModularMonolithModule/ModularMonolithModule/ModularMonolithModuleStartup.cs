using System.Reflection;
using Common.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolithModule.Application;
using ModularMonolithModule.Infrastructure;

namespace ModularMonolithModule;

public static class ModularMonolithModuleStartup
{
    public static async Task InitializeAsync(IConfiguration configuration, bool resetDb)
    {
        var connectionString = configuration.GetDbConnectionString(Schema);
        var assembly = Assembly.GetExecutingAssembly();

        var provider = new ServiceCollection()
            // application
            .AddMediatR(c => c.RegisterServicesFromAssembly(assembly))
            .AddValidatorsFromAssembly(assembly)
            // infrastructure
            .AddScoped<IDbConnectionFactory,DbConnectionFactory>(c=>new DbConnectionFactory(connectionString))
            .AddScoped<IWidgetRepository, WidgetRepository>()
            // builder container
            .BuildServiceProvider();
        CompositionRoot.SetProvider(provider);

        DbMigrations.Apply(Schema, connectionString, assembly, reset: resetDb);
    }
}