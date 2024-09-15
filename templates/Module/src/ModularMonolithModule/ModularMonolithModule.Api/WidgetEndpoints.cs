using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ModularMonolithModule.Application.Queries;

namespace ModularMonolithModule.Api;

public static class WidgetEndpoints
{
    private static readonly ModularMonolithModule Module = new();
    
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/widgets", () =>
            {
                var results = Module.SendQuery(new ListWidgets.Query());
                return Results.Ok(results);
            })
            .WithName("GetWidgets")
            .WithOpenApi();
    }
}