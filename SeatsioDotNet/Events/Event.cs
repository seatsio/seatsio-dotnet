using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace SeatsioDotNet.Events
{
    public class Event
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string ChartKey { get; set; }
        public TableBookingConfig TableBookingConfig { get; set; }
        public bool SupportsBestAvailable { get; set; }
        public ForSaleConfig ForSaleConfig { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public List<Channel> Channels { get; set; }
        public string SocialDistancingRulesetKey { get; set; }
    }
}
