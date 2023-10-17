using System.Collections.Generic;

namespace SeatsioDotNet.Reports.Usage.DetailsForMonth
{
    public class UsageByChart
    {
        public UsageChart Chart { get; set; }
        public IEnumerable<UsageByEvent> UsageByEvent { get; set; }
    }
}