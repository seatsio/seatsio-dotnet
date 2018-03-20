using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class ObjectStatus
    {
        public const string Free = "free";
        public const string Booked = "booked";
        public const string Held = "reservedByToken";

        public string Status { get; set; }
        public string TicketType { get; set; }
        public int? Quantity { get; set; }
        public string HoldToken { get; set; }
        public string OrderId { get; set; }
        public Dictionary<string, object> ExtraData { get; set; }
    }
}