using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularMonolithModule.Infrastructure;
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
            .AddLogging(builder => builder.AddXUnit(this))  
            .BuildServiceProvider();

        var logs = services.GetRequiredService<ILoggerProvider>();
        
        await ModularMonolithModuleStartup.InitializeAsync(configuration, logs, resetDb: true);
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
    public ITestOutputHelper? OutputHelper { get; set; }
}
