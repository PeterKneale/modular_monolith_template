using ModularMonolithModule.Application;
using ModularMonolithModule.Domain;
using ModularMonolithModule.IntegrationTests.Fixtures;
using static ModularMonolithModule.IntegrationTests.TestDataGenerator;

namespace ModularMonolithModule.IntegrationTests;

[Collection(nameof(DbFixture))]
public class WidgetTests
{
    private readonly ModularMonolithModule _module = new();

    [Fact]
    public async Task Can_create()
    {
        var id = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id, name, price));
    }

    [Fact]
    public async Task Cant_create_twice_with_same_id()
    {
        var id = Guid.NewGuid();
        var name1 = UniqueValidName;
        var name2 = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id, name1, price));
        var ex = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
            _module.SendCommand(new CreateWidget.Command(id, name2, price)));
        Assert.Contains($"already exists", ex.Message);
    }

    [Fact]
    public async Task Cant_create_twice_with_same_name()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id1, name, price));
        var ex = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
            _module.SendCommand(new CreateWidget.Command(id2, name, price)));
        Assert.Contains($"already exists", ex.Message);
    }
}