using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ModularMonolithModule.Application.Queries;

namespace ModularMonolithModule.Api;

public static class ModularMonolithModuleEndpoints
{
    private static readonly ModularMonolithModule Module = new();
    
    public static void RegisterEndpoints(this WebApplication app)
    {
        var root = ModularMonolithModuleApiAssemblyInfo.Assembly.GetName().Name;
        var path = $"/{root}/widgets";
        app.MapGet(path, () =>
            {
                var results = Module.SendQuery(new ListWidgets.Query());
                return Results.Ok(results);
            })
            .WithName($"{root} - GetWidgets ")
            .WithOpenApi();
    }
}