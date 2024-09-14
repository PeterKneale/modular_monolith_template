using ModularMonolithModule.Domain;

namespace ModularMonolithModule.UnitTests.Domain;

public class WidgetTest
{
    [Fact]
    public void Can_change_price()
    {                                                                               
        var id = Guid.NewGuid();
        var name = "NAME";
        var price = 1.0m;
        
        var widget = Widget.Create(id, name, price);
        widget.UpdatePrice(2);
        
        Assert.Equal(2, widget.Price);
    }
}