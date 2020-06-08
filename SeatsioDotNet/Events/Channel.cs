using System;
using System.Collections.Generic;
using RestSharp.Deserializers;

namespace SeatsioDotNet.Events
{
    public class Channel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }
        public IEnumerable<string> Objects { get; set; }
    }
}
