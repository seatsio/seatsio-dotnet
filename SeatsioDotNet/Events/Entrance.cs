namespace SeatsioDotNet.Events
{
    public class Entrance
    {
        public string Label { get; set; }

        public Entrance()
        {
        }

        public Entrance(string label = null)
        {
            Label = label;
        }
    }
}