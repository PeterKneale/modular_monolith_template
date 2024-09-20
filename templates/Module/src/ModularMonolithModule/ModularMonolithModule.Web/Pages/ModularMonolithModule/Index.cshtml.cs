using static ModularMonolithModule.Application.Queries.ListWidgets;

namespace ModularMonolithModule.Web.Pages.ModularMonolithModule
{
    public class IndexModel(IModularMonolithModule module) : PageModel
    {
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Widgets = await module.SendQuery(new Query(),cancellationToken);
        }

        public IEnumerable<Response> Widgets { get; set; } = null!;
    }
}
