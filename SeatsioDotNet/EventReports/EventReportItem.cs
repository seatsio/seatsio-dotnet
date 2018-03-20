namespace SeatsioDotNet.EventReports
{
    public class EventReportItem
    {
        public string Label { get; set; }
        public string Status { get; set; }
        public string CategoryLabel { get; set; }
        public int? CategoryKey { get; set; }
        public string TicketType { get; set; }
        public string OrderId { get; set; }
        public bool ForSale { get; set; }
        public string Section { get; set; }
        public string Entrance { get; set; }
        public int? NumBooked { get; set; }
        public int? Capacity { get; set; }
    }
}