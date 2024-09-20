using MediatR;

namespace Common;

public interface IModule
{
    Task SendCommand(IRequest command, CancellationToken token);

    Task<TResult> SendQuery<TResult>(IRequest<TResult> query, CancellationToken token);

    Task PublishNotification(INotification notification, CancellationToken token);
}