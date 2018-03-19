using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class BestAvailableResult
    {
        public Boolean NextToEachOther { get; set; }
        public IEnumerable<string> Objects { get; set; }
    }
}