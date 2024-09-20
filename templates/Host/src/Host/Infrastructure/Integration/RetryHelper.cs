using Polly;
using Polly.Retry;

namespace Host.Infrastructure.Integration;

public static class RetryHelper
{
    private const int RetryAttempts = 3;
    private static readonly TimeSpan RetryInterval = TimeSpan.FromMilliseconds(100);

    public static AsyncRetryPolicy BuildRetryPolicy(ILogger<EventPublisher> logs) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                RetryAttempts,
                retryAttempt => RetryInterval,
                (exception, timeSpan, attempt, context) =>
                    logs.LogWarning($"Attempt {attempt} of {RetryAttempts} failed with exception {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));

    public static AsyncRetryPolicy BuildRetryPolicy(ILogger<QueueProcessor> logs) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                RetryAttempts,
                retryAttempt => RetryInterval,
                (exception, timeSpan, attempt, context) =>
                    logs.LogWarning($"Attempt {attempt} of {RetryAttempts} failed with exception {exception.Message}. Delaying {timeSpan.TotalMilliseconds}ms"));
}