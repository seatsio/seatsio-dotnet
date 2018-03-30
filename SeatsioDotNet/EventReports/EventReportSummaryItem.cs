using System.Collections.Generic;

namespace SeatsioDotNet.EventReports
{
    public class EventReportSummaryItem
    {
        public int Count { get; set; }
        public Dictionary<string, int> byStatus { get; set; }
        public Dictionary<string, int> byCategoryKey { get; set; }
        public Dictionary<string, int> byCategoryLabel { get; set; }
        public Dictionary<string, int> bySection { get; set; }
    }
}