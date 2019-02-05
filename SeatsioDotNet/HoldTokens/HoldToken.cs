using System;
using RestSharp.Deserializers;

namespace SeatsioDotNet.HoldTokens
{
    public class HoldToken
    {
        [DeserializeAs(Name = "holdToken")]
        public string Token { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}