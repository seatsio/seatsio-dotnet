using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class Channel
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Index { get; set; }
    public IEnumerable<string> Objects { get; set; }
    public Dictionary<string, int> AreaPlaces { get; set; }

    public Channel()
    {
    }

    public Channel(string key, string name, string color, int index, IEnumerable<string> objects = null, Dictionary<string, int> areaPlaces = null)
    {
        Key = key;
        Name = name;
        Color = color;
        Index = index;
        Objects = objects;
        AreaPlaces = areaPlaces;
    }
}