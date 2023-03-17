using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageSummaryForMonth
    {
        public UsageMonth Month { get; set; }
        public int NumUsedObjects { get; set; }
    }
}