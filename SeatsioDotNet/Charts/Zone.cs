namespace SeatsioDotNet.Charts;

public class Zone
{
    Zone()
    {
    }

    public Zone(string key, string label)
    {
        this.Key = key;
        this.Label = label;
    }

    public string Key { get; set; }
    public string Label { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != typeof(Zone))
        {
            return false;
        }

        Zone zone = (Zone) obj;
        return Key.Equals(zone.Key) &&
               Label.Equals(zone.Label);
    }
}