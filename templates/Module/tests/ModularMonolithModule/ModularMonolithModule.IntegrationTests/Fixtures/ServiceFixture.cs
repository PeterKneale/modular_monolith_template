using Common;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ModularMonolithModule.IntegrationTests.Fixtures;

public class ServiceFixture : ITestOutputHelperAccessor, IDisposable, IAsyncDisposable
{
    private readonly ServiceProvider _provider;
    private CommonModuleStart _common;
    private readonly IModuleStartup _module;

    public ServiceFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        _provider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging(builder => builder.AddXUnit(this))
            .AddSingleton<IModuleStartup, ModularMonolithModuleStartup>()
            .BuildServiceProvider();

        _common = new CommonModuleStart(configuration);
        _common.Startup();

        _module = _provider.GetRequiredService<IModuleStartup>();
        _module.Startup();
    }
    
    public ITestOutputHelper? OutputHelper { get; set; }

    public void Dispose()
    {
        _provider.Dispose();
        _common.Destroy();
        _module.Destroy();
    }

    public async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
        _common.Destroy();
        _module.Destroy();
    }
}