using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class EventCreationParams
    {
        public string Key { get; set; }
        public bool? BookWholeTables { get; set; }
        public Dictionary<string, string> TableBookingModes { get; set; }
        public string SocialDistancingRulesetKey { get; set; }


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
        }


        public EventCreationParams(string key, Dictionary<string, string> tableBookingModes)
        {
            Key = key;
            TableBookingModes = tableBookingModes;
        }

        public EventCreationParams(string key, string socialDistancingRulesetKey)
        {
            Key = key;
            SocialDistancingRulesetKey = socialDistancingRulesetKey;
        }
    }
}