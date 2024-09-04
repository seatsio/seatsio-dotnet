namespace SeatsioDotNet.Events;

public class Floor
{
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public Floor()
    {
    }
    
    public Floor(string name = null, string displayName = null)
    {
        Name = name;
        DisplayName = displayName;
    }

}