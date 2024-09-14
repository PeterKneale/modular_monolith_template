namespace ModularMonolithModule.IntegrationTests.Fixtures;

public class DbFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await ModularMonolithModuleStartup.InitializeAsync(resetDb: true);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}