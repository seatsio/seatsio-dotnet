using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class BestAvailableResult
    {
        public bool NextToEachOther { get; set; }
        public IEnumerable<string> Objects { get; set; }
    }
}