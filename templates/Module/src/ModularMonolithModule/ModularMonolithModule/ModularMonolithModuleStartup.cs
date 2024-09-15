using System.Reflection;
using Common.Migrations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolithModule.Application;
using ModularMonolithModule.infrastructure;

namespace ModularMonolithModule;

public static class ModularMonolithModuleStartup
{
    public static async Task InitializeAsync(bool resetDb)
    {
        const string schema = "public";
        const string connectionString = "Server=localhost;Database=db;User Id=admin;Password=password";
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

        DatabaseMigrations.Apply(schema, connectionString, assembly, reset: resetDb);
    }
}