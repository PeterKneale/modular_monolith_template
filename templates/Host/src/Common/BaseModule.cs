using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public abstract class BaseModule(Func<IServiceScope> scopeFactory)
{
    public async Task SendCommand(IRequest command)
    {
        using var scope = scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Send(command);
    }

    public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
    {
        using var scope = scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await dispatcher.Send(query);
    }

    public async Task PublishNotification(INotification notification)
    {
        using var scope = scopeFactory.Invoke();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IMediator>();
        await dispatcher.Publish(notification);
    }
}