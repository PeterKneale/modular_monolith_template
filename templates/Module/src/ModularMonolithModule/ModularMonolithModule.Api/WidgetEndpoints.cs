using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ModularMonolithModule.Api;

public static class WidgetEndpoints
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/widgets", () => Results.Ok(new[] { "Widget 1", "Widget 2" }))
            .WithName("GetWidgets")
            .WithOpenApi();
    }
}