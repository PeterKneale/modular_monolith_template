using ModularMonolithModule.Application.Commands;

namespace ModularMonolithModule.Web.Pages.ModularMonolithModule;

public class Create(IModularMonolithModule module) : PageModel
{
    public async Task OnPostAsync(string name, decimal price, CancellationToken cancellationToken)
    {
        var command = new CreateWidget.Command(Guid.NewGuid(),  name, price);
        await module.SendCommand(command, cancellationToken);
    }
}