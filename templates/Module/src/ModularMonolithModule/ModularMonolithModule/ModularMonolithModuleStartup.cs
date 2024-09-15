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
        const string connectionstring = "Server=localhost;Database=db;User Id=admin;Password=password";
        var assembly = Assembly.GetExecutingAssembly();

        var provider = new ServiceCollection()
            // application
            .AddMediatR(c => c.RegisterServicesFromAssembly(assembly))
            .AddValidatorsFromAssembly(assembly)
            // infrastructure
            .AddScoped<IWidgetRepository, WidgetRepository>(c => new WidgetRepository(connectionstring))
            // builder container
            .BuildServiceProvider();
        CompositionRoot.SetProvider(provider);

        DatabaseMigrations.Apply("public", connectionstring, assembly, reset: resetDb);
    }
}