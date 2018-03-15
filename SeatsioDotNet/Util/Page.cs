using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class Page<T>
    {
        public List<T> Items { get; set; }
        public int? NextPageStartsAfter { get; set; }
    }
}