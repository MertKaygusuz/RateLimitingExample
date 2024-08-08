using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateLimitingApi.RequestLogging.Models
{
    public class ResponseLogModel : RequestLogModel
    {
        public required int StatusCode { get; set; }
    }
}