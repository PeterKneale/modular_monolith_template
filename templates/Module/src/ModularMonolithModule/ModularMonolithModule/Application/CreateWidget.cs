using FluentValidation;
using MediatR;
using ModularMonolithModule.Domain;

namespace ModularMonolithModule.Application;

public static class CreateWidget
{
    public record Command(Guid Id, string Name, decimal Price) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Id).NotEmpty();
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