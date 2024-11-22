using System.Collections.Generic;
using System.Linq;

namespace SeatsioDotNet.Events;

public class StatusChangeRequest
{
    public const string CHANGE_STATUS_TO = "CHANGE_STATUS_TO";
    public const string RELEASE = "RELEASE";
    public const string OVERRIDE_SEASON_STATUS = "OVERRIDE_SEASON_STATUS";
    public const string USE_SEASON_STATUS = "USE_SEASON_STATUS";

    public string Type { get; }
    public string EventKey { get; }
    public IEnumerable<ObjectProperties> Objects { get; }
    public string Status { get; }
    public string HoldToken { get; }
    public string OrderId { get; }
    public bool? KeepExtraData { get; }
    public bool? IgnoreChannels { get; }
    public string[] ChannelKeys { get; }
    public string[] AllowedPreviousStatuses { get; }
    public string[] RejectedPreviousStatuses { get; }

    public StatusChangeRequest(string type = CHANGE_STATUS_TO, string eventKey = null, IEnumerable<string> objects = null, string status = null, string holdToken = null, string orderId = null, bool? keepExtraData = null, bool? ignoreChannels = null, string[] channelKeys = null, string[] allowedPreviousStatuses = null, string[] rejectedPreviousStatuses = null)
    {
        Type = type;
        EventKey = eventKey;
        Objects = objects.Select(o => new ObjectProperties(o));
        Status = status;
        HoldToken = holdToken;
        OrderId = orderId;
        KeepExtraData = keepExtraData;
        IgnoreChannels = ignoreChannels;
        ChannelKeys = channelKeys;
        AllowedPreviousStatuses = allowedPreviousStatuses;
        RejectedPreviousStatuses = rejectedPreviousStatuses;
    }
}