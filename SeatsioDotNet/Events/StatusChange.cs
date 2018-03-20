using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class StatusChange
    {
        public int Id { get; set; }
        public string ObjectLabel { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string OrderId { get; set; }
        public long EventId { get; set; }
        public Dictionary<string, object> ExtraData { get; set; }
    }
}