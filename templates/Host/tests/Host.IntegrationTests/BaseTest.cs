using Host.IntegrationTests.Fixtures;

namespace Host.IntegrationTests;

[Collection(nameof(ServiceFixtureCollection))]
public class BaseTest
{
    protected readonly HttpClient Client;
    protected readonly ServiceFixture Service;

    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        Service = service;
        Service.OutputHelper = output;
        Client = service.CreateDefaultClient();
    }
}