using System.Collections.Generic;

namespace SeatsioDotNet.Charts
{
    public class Chart
    {
        public string Key { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public bool Archived { get; set; }
    }
}