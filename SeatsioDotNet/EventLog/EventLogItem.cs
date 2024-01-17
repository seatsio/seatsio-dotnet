using System;
using System.Collections.Generic;

namespace SeatsioDotNet.EventLog;

public class EventLogItem
{
    public long Id { get; set; }
    public string Type { get; set; }
    public string WorkspaceKey { get; set; }
    public DateTimeOffset Date { get; set; }
    public Dictionary<string, object> Data { get; set; }
}