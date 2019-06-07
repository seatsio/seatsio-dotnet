using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageByChart
    {
        public UsageChart Chart { get; set; }
        public IEnumerable<UsageByEvent> UsageByEvent { get; set; }
    }
}