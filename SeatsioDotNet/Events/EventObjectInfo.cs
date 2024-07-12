using System.Collections.Generic;
using SeatsioDotNet.Events;

namespace SeatsioDotNet.EventReports;

public class EventObjectInfo
{
    public const string Available = "available";
    public const string NotAvailable = "not_available";

    public const string NoSection = "NO_SECTION";
    public const string NoZone = "NO_ZONE";
    public const string NoChannel = "NO_CHANNEL";

    public const string Free = "free";
    public const string Booked = "booked";
    public const string Held = "reservedByToken";

    public string Label { get; set; }
    public Labels Labels { get; set; }
    public IDs IDs { get; set; }
    public string Status { get; set; }
    public string CategoryLabel { get; set; }
    public string CategoryKey { get; set; }
    public string TicketType { get; set; }
    public string ObjectType { get; set; }
    public string OrderId { get; set; }
    public bool ForSale { get; set; }
    public string HoldToken { get; set; }
    public string Section { get; set; }
    public string Entrance { get; set; }
    public int? NumBooked { get; set; }
    public int? NumFree { get; set; }
    public int? NumHeld { get; set; }
    public int? NumNotForSale { get; set; }
    public int? Capacity { get; set; }
    public bool? BookAsAWhole { get; set; }
    public Dictionary<string, object> ExtraData { get; set; }
    public bool? IsAccessible { get; set; }
    public bool? IsCompanionSeat { get; set; }
    public bool? HasRestrictedView { get; set; }
    public string DisplayedObjectType { get; set; }
    public string LeftNeighbour { get; set; }
    public string RightNeighbour { get; set; }
    public bool IsAvailable { get; set; }
    public string Channel { get; set; }
    public float? DistanceToFocalPoint { get; set; }
    public Dictionary<string, Dictionary<string, int>> Holds { get; set; }
    public int? NumSeats { get; set; }
    public bool? VariableOccupancy { get; set; }
    public int? MinOccupancy { get; set; }
    public int? MaxOccupancy { get; set; }
    public int SeasonStatusOverriddenQuantity { get; set; }
    public string Zone { get; set; }
}