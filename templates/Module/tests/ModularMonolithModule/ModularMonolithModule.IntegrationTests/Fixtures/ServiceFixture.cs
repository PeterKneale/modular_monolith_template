using Common;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ModularMonolithModule.IntegrationTests.Fixtures;

public class ServiceFixture : IAsyncLifetime,ITestOutputHelperAccessor
{
    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        var services = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging(builder => builder.AddXUnit(this))
            .AddSingleton<IModuleStartup,ModuleStartup>()
            .BuildServiceProvider();

        var startup = services.GetRequiredService<IModuleStartup>();
        await startup.DestroyAsync(); 
        await startup.InitializeAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    public ITestOutputHelper? OutputHelper { get; set; }
}
