using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageByEvent
    {
        public UsageEvent Event { get; set; }
        public int NumUsedObjects { get; set; }
    }
}