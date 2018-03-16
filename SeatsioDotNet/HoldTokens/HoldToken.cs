using RestSharp.Deserializers;

namespace SeatsioDotNet.HoldTokens
{
    public class HoldToken
    {
        [DeserializeAs(Name = "holdToken")]
        public string Token { get; set; }
    }
}