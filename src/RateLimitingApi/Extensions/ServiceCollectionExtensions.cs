using System.Threading.RateLimiting;
using RateLimitingApi.AppSettings;

namespace RateLimitingApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void BuildRateLimit(this IServiceCollection services, AppSetting appSetting)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: partition => new SlidingWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = appSetting.SlidingWindowRateLimitOptions.PermitLimit,
                        SegmentsPerWindow = appSetting.SlidingWindowRateLimitOptions.SegmentsPerWindow,
                        Window = TimeSpan.FromSeconds(appSetting.SlidingWindowRateLimitOptions.WindowFromSeconds)
                    }
                )
            );
        });
    }
}