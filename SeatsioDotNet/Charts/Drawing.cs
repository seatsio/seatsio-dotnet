using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SeatsioDotNet.Charts
{
    public class Drawing
    {
        public string Name { get; set; }
        public string VenueType { get; set; }
        [JsonIgnore]
        public List<Category> Categories => _categories.List;

        [JsonPropertyName("categories")]
        public Categories _categories { get; set; }
    }

    public class Categories
    {
        public List<Category> List { get; set; }
    }
}