using Host.IntegrationTests.Fixtures;

namespace Host.IntegrationTests;

public class HomePageTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Home_page_Should_be_ok() =>
        (await Client.GetAsync("/")).EnsureSuccessStatusCode();
}