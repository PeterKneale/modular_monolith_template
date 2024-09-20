using Common.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Behaviours;

public class TxBehaviour<TRequest, TResponse>(IDbConnectionFactory connections, ILogger<TRequest> log) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        log.LogInformation("Beginning transaction");
        var connection = await connections.OpenAsync();
        var tx = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await next();
            log.LogInformation("Committing transaction");
            await tx.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            log.LogError(e, "Error executing transaction");
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}