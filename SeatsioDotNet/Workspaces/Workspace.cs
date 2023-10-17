namespace SeatsioDotNet.Events;

public class Workspace
{
    public string Name { get; set; }
    public string Key { get; set; }
    public string SecretKey { get; set; }
    public bool IsTest { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}