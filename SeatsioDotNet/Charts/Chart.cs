using System.Collections.Generic;
using SeatsioDotNet.Events;

namespace SeatsioDotNet.Charts;

public class Chart
{
    public long Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public IEnumerable<string> Tags { get; set; }
    public bool Archived { get; set; }
    public string PublishedVersionThumbnailUrl { get; set; }
    public string DraftVersionThumbnailUrl { get; set; }
    public IEnumerable<Event> Events { get; set; }
    public ChartValidationResult Validation { get; set; }
    public string VenueType { get; set; }
}