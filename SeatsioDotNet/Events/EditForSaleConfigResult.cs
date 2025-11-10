using System;

namespace SeatsioDotNet.Events;

public class EditForSaleConfigResult
{
    public ForSaleRateLimitInfo RateLimitInfo { get; set; }
    public ForSaleConfig ForSaleConfig { get; set; }
}

public class ForSaleRateLimitInfo
{
    public int RateLimitRemainingCalls { get; set; }
    public DateTimeOffset RateLimitResetDate { get; set; }
}