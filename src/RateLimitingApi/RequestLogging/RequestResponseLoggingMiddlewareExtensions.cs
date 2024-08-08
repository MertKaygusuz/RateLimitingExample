namespace RateLimitingApi.RequestLogging
{
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiRequestResponseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRequestResponseCustomMiddleware>();
        }
    }
}