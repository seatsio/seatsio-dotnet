using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class ChannelCreationParams
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Index { get; set; }
    public string[] Objects { get; set; }
    public Dictionary<string, int> AreaPlaces { get; set; }

    public ChannelCreationParams(string key, string name, string color, int index = 0, string[] objects = null, Dictionary<string, int> areaPlaces = null)
    {
        Key = key;
        Name = name;
        Color = color;
        Index = index;
        Objects = objects;
        AreaPlaces = areaPlaces;
    }
}