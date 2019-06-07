using System;

namespace SeatsioDotNet.UsageReports.DetailsForEventInMonth
{
    public class UsageForObject
    {
        public string Object { get; set; }
        public int NumFirstBookings { get; set; }
        public DateTimeOffset FirstBookingDate { get; set; }
        public int NumFirstSelections { get; set; }
        public int NumFirstBookingsOrSelections { get; set; }

    }
}