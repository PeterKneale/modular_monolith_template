using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolithModule.Application;
using ModularMonolithModule.infrastructure;

namespace ModularMonolithModule;

public static class ModularMonolithModuleStartup
{
    public static async Task InitializeAsync(bool resetDb)
    {
        var connectionstring = "Server=localhost;Database=db;User Id=admin;Password=password";
        var assemblies = new[] { Assembly.GetExecutingAssembly() };

        var provider = new ServiceCollection()
            // application
            .AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies))
            .AddValidatorsFromAssemblies(assemblies)
            // infrastructure
            .AddSingleton<Schema>()
            .AddScoped<IWidgetRepository, WidgetRepository>(c => new WidgetRepository(connectionstring))
            // builder container
            .BuildServiceProvider();
        CompositionRoot.SetProvider(provider);

        if (resetDb)
        {
            await Schema.DropAsync(connectionstring);
        }

        await Schema.CreateAsync(connectionstring);
    }
}