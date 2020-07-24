using System.Collections.Generic;
using System.Linq;

namespace SeatsioDotNet.Events
{
    public class StatusChangeRequest
    {
        public string EventKey { get; }
        public IEnumerable<ObjectProperties> Objects { get; }
        public string Status { get; }
        public string HoldToken { get; }
        public string OrderId { get; }
        public bool? KeepExtraData { get; }
        public bool? IgnoreChannels { get;  }
        public string[] ChannelKeys { get;  }

        public StatusChangeRequest(string eventKey, IEnumerable<ObjectProperties> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            EventKey = eventKey;
            Objects = objects;
            Status = status;
            HoldToken = holdToken;
            OrderId = orderId;
            KeepExtraData = keepExtraData;
            IgnoreChannels = ignoreChannels;
            ChannelKeys = channelKeys;
        }

        public StatusChangeRequest(string eventKey, IEnumerable<string> objects, string status, string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null)
        {
            EventKey = eventKey;
            Objects = objects.Select(o => new ObjectProperties(o));
            Status = status;
            HoldToken = holdToken;
            OrderId = orderId;
            KeepExtraData = keepExtraData;
            IgnoreChannels = ignoreChannels;
            ChannelKeys = channelKeys;
        }
    }
}