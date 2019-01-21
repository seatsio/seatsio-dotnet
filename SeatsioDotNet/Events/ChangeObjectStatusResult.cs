using System.Collections.Generic;
using SeatsioDotNet.EventReports;

namespace SeatsioDotNet.Events
{
    public class ChangeObjectStatusResult
    {
        public Dictionary<string, EventReportItem> Objects { get; set; }
    }
}