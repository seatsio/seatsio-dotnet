using System.Collections.Generic;

namespace SeatsioDotNet.Reports.Events;

public class EventReportDeepSummaryItem
{
    public int Count { get; set; }
    public Dictionary<string, EventReportSummaryItem> byStatus { get; set; }
    public Dictionary<string, EventReportSummaryItem> byCategoryKey { get; set; }
    public Dictionary<string, EventReportSummaryItem> byCategoryLabel { get; set; }
    public Dictionary<string, EventReportSummaryItem> bySection { get; set; }
    public Dictionary<string, EventReportSummaryItem> byAvailability { get; set; }
    public Dictionary<string, EventReportSummaryItem> byChannel { get; set; }
}