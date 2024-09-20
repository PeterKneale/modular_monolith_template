using Xunit.Abstractions;

namespace ModularMonolithModule.IntegrationTests.Fixtures;

[Collection(nameof(ServiceFixture))]
public class BaseTest
{
    protected BaseTest(ServiceFixture service, ITestOutputHelper output)
    {
        Service = service;
        service.OutputHelper = output;
        ModuleModule = new ModularMonolithModuleModule();
    }

    protected ServiceFixture Service { get; init; }
    
    protected ModularMonolithModuleModule ModuleModule { get; init; }
}