namespace ModularMonolithModule.Domain;

public class Widget
{
    private Widget(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Widget Create(Guid id, string name, decimal price) =>
        new(id, name, price);

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }
}