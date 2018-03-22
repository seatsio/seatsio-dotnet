using System.Collections.Generic;
using RestSharp.Deserializers;

namespace SeatsioDotNet.Charts
{
    public class Drawing
    {
        public string Name { get; set; }
        public string VenueType { get; set; }
        public List<Category> Categories => _categories.List;

        [DeserializeAs(Name = "categories")]
        public Categories _categories { get; set; }
    }

    public class Categories
    {
        public List<Category> List { get; set; }
    }
}