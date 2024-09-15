namespace Host.IntegrationTests.Fixtures;

public class ServiceFixture : WebApplicationFactory<HostAssemblyInfo>, ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(p => p.AddXUnit(this));
    }
}