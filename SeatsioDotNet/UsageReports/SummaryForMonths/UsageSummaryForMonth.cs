using System.Collections.Generic;

namespace SeatsioDotNet.UsageReports
{
    public class UsageSummaryForMonth
    {
        public UsageMonth Month { get; set; }
        public int NumUsedObjects { get; set; }
        public int NumFirstBookings { get; set; }
        public Dictionary<string, int> NumFirstBookingsByStatus { get; set; }
        public int NumFirstBookingsOrSelections { get; set; }
    }
}