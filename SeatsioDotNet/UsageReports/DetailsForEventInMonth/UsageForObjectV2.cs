using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports.DetailsForEventInMonth
{
    public class UsageForObjectV2
    {
        public string Object { get; set; }
        public int NumUsedObjects { get; set; }
        public Dictionary<string, int> UsageByReason { get; set; }

    }
}