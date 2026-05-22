using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class Channel
{
    public string Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Index { get; set; }
    public IEnumerable<string> Objects { get; set; }
    public Dictionary<string, int> AreaPlaces { get; set; }

    public Channel()
    {
    }

    public Channel(string key, string id, string name, string color, int index, IEnumerable<string> objects, Dictionary<string, int> areaPlaces)
    {
        Key = key;
        Id = id;
        Name = name;
        Color = color;
        Index = index;
        Objects = objects;
        AreaPlaces = areaPlaces;
    }

    public string AreaPartitionLabel(string areaLabel)
    {
        return $"{areaLabel}##{Id}";
    }

    public Dictionary<string, object> ToJsonObject()
    {
        var dict = new Dictionary<string, object>();
        dict.Add("key", Key);
        dict.Add("name", Name);
        dict.Add("color", Color);
        dict.Add("index", Index);
        dict.Add("objects", Objects);
        dict.Add("areaPlaces", AreaPlaces);
        return dict;
    }
}