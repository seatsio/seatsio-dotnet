using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class Page<T>
    {
        public List<T> Items { get; set; }
        public long? NextPageStartsAfter { get; set; }
        public long? PreviousPageEndsBefore { get; set; }
    }
}