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

    public Channel(string key, string name, string color, int index, IEnumerable<string> objects, Dictionary<string, int> areaPlaces = null)
    {
        Key = key;
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
        if (Key != null) dict.Add("key", Key);
        if (Name != null) dict.Add("name", Name);
        if (Color != null) dict.Add("color", Color);
        dict.Add("index", Index);
        if (Objects != null) dict.Add("objects", Objects);
        if (AreaPlaces != null && AreaPlaces.Count > 0) dict.Add("areaPlaces", AreaPlaces);
        return dict;
    }
}