namespace SeatsioDotNet.Events;

public class IDs
{
    public string Own { get; set; }
    public string Parent { get; set; }
    public string Section { get; set; }

    public IDs()
    {
    }

    public IDs(string own, string parent, string section)
    {
        Own = own;
        Parent = parent;
        Section = section;
    }
}