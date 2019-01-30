using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class EventCreationParams
    {
        public string Key { get; set; }
        public bool? BookWholeTables { get; set; }
        public Dictionary<string, string> TableBookingModes { get; set; }

        public EventCreationParams()
        {
            
        }

        public EventCreationParams(string key)
        {
            Key = key;
        }

        public EventCreationParams(string key, bool bookWholeTables)
        {
            Key = key;
            BookWholeTables = bookWholeTables;
            TableBookingModes = null;
        }

        public EventCreationParams(string key, Dictionary<string, string> tableBookingModes)
        {
            Key = key;
            TableBookingModes = tableBookingModes;
            BookWholeTables = null;
        }


    }
}