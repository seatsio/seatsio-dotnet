using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class Channel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }
        public IEnumerable<string> Objects { get; set; }

        public Channel() {}

        public Channel(string name, string color, int index)
        {
            Name = name;
            Color = color;
            Index = index;
        }

        public object AsJsonObject()
        {
            return new
            {
                name = Name,
                index = Index,
                color = Color
            };
        }

    }
}
