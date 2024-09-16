using System.Reflection;
using Common;
using Common.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularMonolithModule.Application;
using ModularMonolithModule.Infrastructure;

namespace ModularMonolithModule;

public static class ModularMonolithModuleStartup
{
    public static async Task InitializeAsync(IConfiguration configuration, ILoggerProvider logs, bool resetDb)
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
        
        CompositionRoot.SetProvider(provider);

        DbMigrations.Apply(Schema, connectionString, assembly, reset: resetDb);
        
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}