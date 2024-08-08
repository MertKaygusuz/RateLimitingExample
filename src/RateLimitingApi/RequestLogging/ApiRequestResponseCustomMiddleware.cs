using Microsoft.Extensions.Caching.Distributed;
using RateLimitingApi.Extensions;
using RateLimitingApi.RequestLogging.Models;

namespace RateLimitingApi.RequestLogging
{
    public class ApiRequestResponseCustomMiddleware(RequestDelegate next, IDistributedCache memoryCache)
    {
        private readonly RequestDelegate _next = next;

        private readonly IDistributedCache _memoryCache = memoryCache;

        public async Task Invoke(HttpContext context)
        {  
            Guid RequestId = Guid.NewGuid();

            var requestDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine($"(On Request)  Request Path: {context.Request.Path}. Request Id: {RequestId}. Date: {requestDate}. Ip Address: {context.Connection.RemoteIpAddress}");
            var requestLogModel = new RequestLogModel() 
            {
                Path = context.Request.Path,
                RequestId = RequestId,
                UnixTimeInMilliseconds = requestDate,
                RemoteIpAddress = context.Connection.RemoteIpAddress.ToString()
            };
            var oldRequestCaches = await _memoryCache.GetRecordAsync<List<RequestLogModel>>(ApiCacheExtensions.requestKey);
            oldRequestCaches ??= [];
            oldRequestCaches.Add(requestLogModel);
            await _memoryCache.SaveRecordAsync(ApiCacheExtensions.requestKey, oldRequestCaches);

            
            await _next(context);


            var responseDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var responseLogModel = new ResponseLogModel() 
            {
                Path = context.Request.Path,
                RequestId = RequestId,
                UnixTimeInMilliseconds = responseDate,
                RemoteIpAddress = context.Connection.RemoteIpAddress.ToString(),
                StatusCode = context.Response.StatusCode
            };
            var oldResponseCaches = await _memoryCache.GetRecordAsync<List<ResponseLogModel>>(ApiCacheExtensions.responseKey);
            oldResponseCaches ??= [];
            oldResponseCaches.Add(responseLogModel);
            await _memoryCache.SaveRecordAsync(ApiCacheExtensions.responseKey, oldResponseCaches);
            Console.WriteLine($"(On Response) Request Path: {context.Request.Path}. Request Id: {RequestId}. Date: {responseDate}. Ip Address: {context.Connection.RemoteIpAddress}. Response status code: {context.Response.StatusCode}");
        }
                
    }
}