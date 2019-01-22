using System.Collections.Generic;
using SeatsioDotNet.EventReports;

namespace SeatsioDotNet.Events
{
    public class BestAvailableResult
    {
        public bool NextToEachOther { get; set; }
        public IEnumerable<string> Objects { get; set; }
        public Dictionary<string, EventReportItem> ObjectDetails { get; set; }
    }
}