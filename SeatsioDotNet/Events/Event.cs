using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string ChartKey { get; set; }
        public bool BookWholeTables { get; set; }
        public Dictionary<string, string> TableBookingModes { get; set; }
        public bool SupportsBestAvailable { get; set; }
        public ForSaleConfig ForSaleConfig { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}