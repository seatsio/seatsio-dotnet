namespace SeatsioDotNet.Events
{
    public class EventCreationParams
    {
        public string Key { get; set; }

        public EventCreationParams()
        {
            
        }

        public EventCreationParams(string key)
        {
            Key = key;
        }
            
            
    }
}