namespace ModularMonolithModule.Application.Commands;

public static class CreateWidget
{
    public record Command(Guid Id, string Name, decimal Price) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(255);
            RuleFor(m => m.Price).GreaterThan(0).LessThan(1000000);
        }
    }

    public class Handler(IWidgetRepository repository) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var id = command.Id;
            var name = command.Name;
            var price = command.Price;
            
            if(await repository.Exists(id))
            {
                BusinessRuleValidationException.ThrowAlreadyExists<Widget>(id);
            }
        
            if(await repository.Exists(name))
            {
                BusinessRuleValidationException.ThrowAlreadyExists<Widget>(name);
            }
        
            var widget = Widget.Create(id, name, price);
            await repository.Add(widget);
        }
    }
}