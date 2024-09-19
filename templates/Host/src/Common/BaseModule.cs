using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public abstract class BaseModule(Func<IServiceScope> scopes)
{
    public async Task SendCommand(IRequest command)
    {
        Debug.Assert(command != null, "command!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        Debug.Assert(query != null, "query!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }

    public async Task PublishNotification(INotification notification)
    {
        Debug.Assert(notification != null, "notification!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification);
    }
}