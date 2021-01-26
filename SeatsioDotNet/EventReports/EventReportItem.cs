using System.Collections.Generic;
using SeatsioDotNet.Events;

namespace SeatsioDotNet.EventReports
{
    public class EventReportItem
    {
        public const string Selectable = "selectable";
        public const string NotSelectable = "not_selectable";

        public const string NoSection = "NO_SECTION";
        public const string NoChannel = "NO_CHANNEL";

        public string Label { get; set; }
        public Labels Labels { get; set; }
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
        public int? Capacity { get; set; }
        public bool? BookAsAWhole { get; set; }
        public Dictionary<string, object> ExtraData { get; set; }
        public bool? IsAccessible { get; set; }
        public bool? IsCompanionSeat { get; set; }
        public bool? HasRestrictedView { get; set; }
        public string DisplayedObjectType { get; set; }
        public string LeftNeighbour { get; set; }
        public string RightNeighbour { get; set; }
        public bool IsSelectable { get; set; }
        public bool? IsDisabledBySocialDistancing { get; set; }
        public string Channel { get; set; }
    }
}
