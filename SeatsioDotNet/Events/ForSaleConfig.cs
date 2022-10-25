using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class ForSaleConfig
    {
        public bool ForSale { get; set; }
        public IEnumerable<string> Objects { get; set; }
        public Dictionary<string, int> AreaPlaces { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}