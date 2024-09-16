using Xunit.Abstractions;

namespace ModularMonolithModule.IntegrationTests.Fixtures;

[Collection(nameof(ServiceFixture))]
public class BaseTest
{
    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        Service = service;
        service.OutputHelper = output;
        Module = new ModularMonolithModule();
    }

    protected ServiceFixture Service { get; init; }
    
    protected ModularMonolithModule Module { get; init; }
}