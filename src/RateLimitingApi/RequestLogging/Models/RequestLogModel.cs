using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateLimitingApi.RequestLogging.Models
{
    public class RequestLogModel
    {
        public required string Path { get; set; }

        public required Guid RequestId { get; set; }

        public required long UnixTimeInMilliseconds { get; set; }

        public required string RemoteIpAddress { get; set; }
    }
}