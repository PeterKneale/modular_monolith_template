using Microsoft.Extensions.Configuration;

namespace ModularMonolithModule.IntegrationTests.Fixtures;

public class DbFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        
        await ModularMonolithModuleStartup.InitializeAsync(configuration, resetDb: true);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
