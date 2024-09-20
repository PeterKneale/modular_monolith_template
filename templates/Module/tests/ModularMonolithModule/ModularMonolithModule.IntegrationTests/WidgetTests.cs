using ModularMonolithModule.Application.Commands;
using ModularMonolithModule.Application.Queries;
using ModularMonolithModule.Domain;
using ModularMonolithModule.IntegrationTests.Fixtures;
using Xunit.Abstractions;
using static ModularMonolithModule.IntegrationTests.TestDataGenerator;

namespace ModularMonolithModule.IntegrationTests;

[Collection(nameof(ServiceFixture))]
public class WidgetTests(ServiceFixture service, ITestOutputHelper output) : BaseTest(service, output)
{
    [Fact]
    public async Task Can_create_then_get_by_id()
    {
        var id = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await ModuleModule.SendCommand(new CreateWidget.Command(id, name, price), CancellationToken.None);
        
        var result = await ModuleModule.SendQuery(new GetWidget.Query(id), CancellationToken.None);
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

        await ModuleModule.SendCommand(new CreateWidget.Command(id, name, price), CancellationToken.None);
        
        var results = await ModuleModule.SendQuery(new ListWidgets.Query(), CancellationToken.None);
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

        await ModuleModule.SendCommand(new CreateWidget.Command(id, name, price), CancellationToken.None);
        var result1 = await ModuleModule.SendQuery(new GetWidgetName.Query(id), CancellationToken.None);
        Assert.Equal(name, result1);
    }

    [Fact]
    public async Task Cant_create_twice_with_same_id()
    {
        var id = Guid.NewGuid();
        var name1 = UniqueValidName;
        var name2 = UniqueValidName;
        var price = ValidPrice;

        await ModuleModule.SendCommand(new CreateWidget.Command(id, name1, price), CancellationToken.None);
        var ex = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
            ModuleModule.SendCommand(new CreateWidget.Command(id, name2, price), CancellationToken.None));
        Assert.Contains($"already exists", ex.Message);
    }

    [Fact]
    public async Task Cant_create_twice_with_same_name()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var name = UniqueValidName;
        var price = ValidPrice;

        await ModuleModule.SendCommand(new CreateWidget.Command(id1, name, price), CancellationToken.None);
        var ex = await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
            ModuleModule.SendCommand(new CreateWidget.Command(id2, name, price), CancellationToken.None));
        Assert.Contains($"already exists", ex.Message);
    }
}