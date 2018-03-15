using System.Collections.Generic;

namespace SeatsioDotNet.Test.Events
{
    public class ObjectStatus
    {
        public const string Free = "free";
        public const string Booked = "booked";
        public const string Held = "reservedByToken";
        
        public string Status { get; set; }
    }
}