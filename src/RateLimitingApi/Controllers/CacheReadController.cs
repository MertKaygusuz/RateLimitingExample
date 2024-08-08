using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Distributed;
using RateLimitingApi.Extensions;
using RateLimitingApi.RequestLogging.Models;
using RateLimitingApi.ResponseDtos;

namespace RateLimitingApi.Controllers
{
    [ApiController]
    [DisableRateLimiting]
    [Route("[controller]/[action]")]
    public class CacheReadController(IDistributedCache memoryCache) : ControllerBase
    {
        private readonly IDistributedCache _memoryCache = memoryCache;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RequestLogDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<RequestLogDto>> GetRequestLogs()
        {
            var rawRequestData = await _memoryCache.GetRecordAsync<List<RequestLogModel>>(ApiCacheExtensions.requestKey);

            if (rawRequestData is null)
                return null;


            return rawRequestData.Select(x =>
                                            new RequestLogDto
                                            {
                                                Path = x.Path,
                                                RequestId = x.RequestId,
                                                UnixTimeInMilliseconds = x.UnixTimeInMilliseconds,
                                                RemoteIpAddress = x.RemoteIpAddress
                                            })
                                  .OrderBy(x => x.UnixTimeInMilliseconds);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseLogDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ResponseLogDto>> GetResponseLogs()
        {
            var rawResponseData = await _memoryCache.GetRecordAsync<List<ResponseLogModel>>(ApiCacheExtensions.responseKey);

            if (rawResponseData is null)
                return null;


            return rawResponseData.Select(x =>
                                            new ResponseLogDto
                                            {
                                                Path = x.Path,
                                                RequestId = x.RequestId,
                                                UnixTimeInMilliseconds = x.UnixTimeInMilliseconds,
                                                RemoteIpAddress = x.RemoteIpAddress,
                                                StatusCode = x.StatusCode
                                            })
                                    .OrderBy(x => x.UnixTimeInMilliseconds);
        }

    }
}