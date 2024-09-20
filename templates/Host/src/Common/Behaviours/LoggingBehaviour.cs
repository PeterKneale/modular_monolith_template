using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logs) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = GetName();
        var body = JsonConvert.SerializeObject(request);

        logs.LogInformation("Executing: {Name} - {Body}", name, body);

        TResponse result;
        try
        {
            result = await next();
        }
        catch (Exception e)
        {
            logs.LogError(e, "Error Executing: {Name} - {Body}", name, body);
            throw;
        }

        return result;
    }

    private static string GetName() => typeof(TRequest).FullName!.Split(".").Last();
}