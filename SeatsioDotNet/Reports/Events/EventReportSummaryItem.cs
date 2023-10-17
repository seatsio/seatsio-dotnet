using System.Collections.Generic;

namespace SeatsioDotNet.Reports.Events;

public class EventReportSummaryItem
{
    public int Count { get; set; }
    public Dictionary<string, int> byStatus { get; set; }
    public Dictionary<string, int> byCategoryKey { get; set; }
    public Dictionary<string, int> byCategoryLabel { get; set; }
    public Dictionary<string, int> bySection { get; set; }
    public Dictionary<string, int> byAvailability { get; set; }
    public Dictionary<string, int> byChannel { get; set; }
}