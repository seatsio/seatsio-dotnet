using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SeatsioDotNet.Util
{
    public class Page<T>
    {
        public List<T> Items { get; set; }
        [JsonPropertyName("next_page_starts_after")]
        public long? NextPageStartsAfter { get; set; }
        [JsonPropertyName("previous_page_ends_before")]
        public long? PreviousPageEndsBefore { get; set; }
    }
}