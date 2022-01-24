using System.Collections.Generic;
using SeatsioDotNet.Events;

namespace SeatsioDotNet.Seasons
{
    public class Season
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public List<string> PartialSeasonKeys { get; set; }
        public Event SeasonEvent { get; set; }
        public List<Event> Events { get; set; }
    }
}