using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class ForSaleConfig
    {
        public bool ForSale { get; set; }
        public IEnumerable<string> Objects { get; set; }
        public Dictionary<string, int> AreaPlaces { get; set; }
        public IEnumerable<string> Categories { get; set; }
        
        public ForSaleConfig WithForSale(bool forSale)
        {
            ForSale = forSale;
            return this;
        }

        public ForSaleConfig WithObjects(IEnumerable<string> objects)
        {
            Objects = objects;
            return this;
        }

        public ForSaleConfig WithAreaPlaces(Dictionary<string, int> areaPlaces)
        {
            AreaPlaces = areaPlaces;
            return this;
        }

        public ForSaleConfig WithCategories(IEnumerable<string> categories)
        {
            Categories = categories;
            return this;
        }

        public object AsJsonObject()
        {
            return new
            {
                forSale = ForSale,
                objects = Objects,
                areaPlaces = AreaPlaces,
                categories = Categories
            };
        }
    }
}