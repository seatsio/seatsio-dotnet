using System.Collections.Generic;

namespace SeatsioDotNet.ChartReports
{
    public class ChartReportSummaryItem
    {
        public int Count { get; set; }
        public Dictionary<string, int> byCategoryKey { get; set; }
        public Dictionary<string, int> byCategoryLabel { get; set; }
        public Dictionary<string, int> bySection { get; set; }
        public Dictionary<string, int> byObjectType { get; set; }
    }
}