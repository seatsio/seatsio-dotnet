using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;

namespace SeatsioDotNet.Events;

public class Event
{
    public long Id { get; set; }
    public string Key { get; set; }
    public string ChartKey { get; set; }
    public string Name { get; set; }
    public DateOnly Date { get; set; }
    public TableBookingConfig TableBookingConfig { get; set; }
    public bool SupportsBestAvailable { get; set; }
    public ForSaleConfig ForSaleConfig { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public List<Channel> Channels { get; set; }
    public Dictionary<string, object> ObjectCategories { get; set; }
    public List<Category> Categories { get; set; }
    public List<string> PartialSeasonKeys { get; set; }
    public List<Event> Events { get; set; }
    public bool IsSeason { get; set; }
    public bool IsTopLevelSeason { get; set; }
    public bool IsPartialSeason { get; set; }
    public bool IsEventInSeason { get; set; }
    public string TopLevelSeasonKey { get; set; }
    public bool IsInThePast { get; set; }
    public string[] PartialSeasonKeysForEvent { get; set; }
}