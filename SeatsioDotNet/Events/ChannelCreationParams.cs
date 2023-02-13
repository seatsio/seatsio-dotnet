namespace SeatsioDotNet.Events;

public class ChannelCreationParams
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Index { get; set; }
    public string[] Objects { get; set; }

    public ChannelCreationParams(string key, string name, string color)
    {
        Key = key;
        Name = name;
        Color = color;
    }

    public ChannelCreationParams(string key, string name, string color, string[] objects)
    {
        Key = key;
        Name = name;
        Color = color;
        Objects = objects;
    }

    public ChannelCreationParams(string key, string name, string color, int index, string[] objects)
    {
        Key = key;
        Name = name;
        Color = color;
        Index = index;
        Objects = objects;
    }
}