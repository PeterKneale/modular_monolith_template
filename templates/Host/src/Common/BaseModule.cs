using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public abstract class BaseModule(Func<IServiceScope> scopes)
{
    public async Task SendCommand(IRequest command, CancellationToken token)
    {
        Debug.Assert(command != null, "command!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command, token);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query, CancellationToken token)
    {
        Debug.Assert(query != null, "query!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query, token);
    }

    public async Task PublishNotification(INotification notification, CancellationToken token)
    {
        Debug.Assert(notification != null, "notification!=null");
        using var scope = scopes.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification, token);
    }
}