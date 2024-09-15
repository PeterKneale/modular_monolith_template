using ModularMonolithModule.Application.Commands;
using ModularMonolithModule.Application.Queries;
using ModularMonolithModule.Domain;
using ModularMonolithModule.IntegrationTests.Fixtures;
using static ModularMonolithModule.IntegrationTests.TestDataGenerator;

namespace ModularMonolithModule.IntegrationTests;

[Collection(nameof(DbFixture))]
public class WidgetTests
{
    private readonly ModularMonolithModule _module = new();

    [Fact]
    public async Task Can_create_then_get_by_id()
    {
        var id = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id, name, price));
        
        var result = await _module.SendQuery(new GetWidget.Query(id));
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(price, result.Price);
    }
    
    [Fact]
    public async Task Can_create_then_find_in_list()
    {
        var id = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id, name, price));
        
        var results = await _module.SendQuery(new ListWidgets.Query());
        Assert.Contains(id, results.Select(x => x.Id));
        var result = results.Single(x=>x.Id == id);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(price, result.Price);
    }

    [Fact]
    public async Task Can_create_then_get_name_by_id()
    {
        var id = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await _module.SendCommand(new CreateWidget.Command(id, name, price));
        var result1 = await _module.SendQuery(new GetWidgetName.Query(id));
        Assert.Equal(name, result1);
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