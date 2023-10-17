using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class StatusChange
{
    public long Id { get; set; }
    public string ObjectLabel { get; set; }
    public string Status { get; set; }
    public DateTimeOffset Date { get; set; }
    public string OrderId { get; set; }
    public long EventId { get; set; }
    public Dictionary<string, object> ExtraData { get; set; }
    public StatusChangeOrigin Origin { get; set; }
    public bool IsPresentOnChart { get; set; }
    public string NotPresentOnChartReason { get; set; }
    public string HoldToken { get; set; }
}