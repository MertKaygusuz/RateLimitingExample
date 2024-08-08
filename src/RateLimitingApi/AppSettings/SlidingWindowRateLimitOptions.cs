namespace RateLimitingApi.AppSettings;

public class SlidingWindowRateLimitOptions
{
    public int PermitLimit { get; set; }
    public int SegmentsPerWindow { get; set; }
    public int WindowFromSeconds { get; set; }
}