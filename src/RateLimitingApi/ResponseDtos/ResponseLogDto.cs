namespace RateLimitingApi.ResponseDtos
{
    public class ResponseLogDto
    {
        public required string Path { get; set; }

        public required Guid RequestId { get; set; }

        public required long UnixTimeInMilliseconds { get; set; }

        public required string RemoteIpAddress { get; set; }

        public required int StatusCode { get; set; }

        public DateTime TimeInLocal { get => DateTimeOffset.FromUnixTimeMilliseconds(UnixTimeInMilliseconds).LocalDateTime; }
        public DateTime TimeInUtc { get => DateTimeOffset.FromUnixTimeMilliseconds(UnixTimeInMilliseconds).UtcDateTime; }
    }
}