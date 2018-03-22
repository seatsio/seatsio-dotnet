using System.Collections.Generic;

namespace SeatsioDotNet.Charts
{
    public class Category
    {
        public int? Key { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }

        public Category()
        {
        }

        public Category(int? key, string label, string color)
        {
            Key = key;
            Label = label;
            Color = color;
        }

        public Dictionary<string, object> AsDictionary()
        {
            var dictionary = new Dictionary<string, object>();

            if (Key != null)
            {
                dictionary.Add("key", Key);
            }

            if (Label != null)
            {
                dictionary.Add("label", Label);
            }

            if (Color != null)
            {
                dictionary.Add("color", Color);
            }

            return dictionary;
        }
    }
}