using System.Globalization;
using System.Net;
using Polly;
using Polly.Retry;
using RestSharp;

namespace Apps.Lionbridge.Api;

public static class LionbridgePollyPolicies
{
    private static readonly TimeSpan MinDelay = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(30);

    public static ResiliencePipeline<RestResponse> GetRetryPolicy(int retryCount = 5)
    {
        var retryOptions = new RetryStrategyOptions<RestResponse>
        {
            MaxRetryAttempts = retryCount,
            ShouldHandle = new PredicateBuilder<RestResponse>()
                .HandleResult(response => response.StatusCode == HttpStatusCode.TooManyRequests ||
                                          response.StatusCode == HttpStatusCode.ServiceUnavailable),
            DelayGenerator = args =>
            {
                var retryAfter = TryGetRetryAfter(args.Outcome.Result);
                if (retryAfter.HasValue)
                    return new ValueTask<TimeSpan?>(retryAfter.Value);

                var exponentialSeconds = Math.Min(MinDelay.TotalSeconds * Math.Pow(2, args.AttemptNumber),
                    MaxDelay.TotalSeconds);
                var jitterSeconds = Random.Shared.NextDouble();
                var delay = TimeSpan.FromSeconds(Math.Min(exponentialSeconds + jitterSeconds, MaxDelay.TotalSeconds));

                return new ValueTask<TimeSpan?>(delay);
            }
        };

        return new ResiliencePipelineBuilder<RestResponse>()
            .AddRetry(retryOptions)
            .Build();
    }

    private static TimeSpan? TryGetRetryAfter(RestResponse? response)
    {
        var retryAfterValue = response?.Headers?
            .FirstOrDefault(header => header.Name?.Equals("Retry-After", StringComparison.OrdinalIgnoreCase) == true)
            ?.Value?.ToString();

        if (string.IsNullOrWhiteSpace(retryAfterValue))
            return null;

        if (double.TryParse(retryAfterValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var seconds))
            return TimeSpan.FromSeconds(Math.Min(seconds, MaxDelay.TotalSeconds));

        if (DateTimeOffset.TryParse(retryAfterValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                out var retryAt))
        {
            var delay = retryAt - DateTimeOffset.UtcNow;
            if (delay > TimeSpan.Zero)
                return delay > MaxDelay ? MaxDelay : delay;
        }

        return null;
    }
}
