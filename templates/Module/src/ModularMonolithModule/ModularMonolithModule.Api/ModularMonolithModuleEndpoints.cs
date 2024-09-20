using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ModularMonolithModule.Application.Queries;

namespace ModularMonolithModule.Api;

public static class ModularMonolithModuleEndpoints
{
    private static readonly ModularMonolithModuleModule ModuleModule = new();
    
    public static void UseModuleEndpoints(this WebApplication app)
    {
        var root = ModularMonolithModuleApiAssemblyInfo.Assembly.GetName().Name;
        var path = $"/{root}/widgets";
        app.MapGet(path, async (CancellationToken token) =>
            {
                var results = await ModuleModule.SendQuery(new ListWidgets.Query(),token);
                return Results.Ok(results);
            })
            .WithName($"{root} - GetWidgets ")
            .WithOpenApi();
    }
}