using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class EventCreationParams
    {
        public string Key { get; set; }
        public TableBookingConfig TableBookingConfig { get; set; }
        public string SocialDistancingRulesetKey { get; set; }


        public EventCreationParams()
        {
        }

        public EventCreationParams(string key)
        {
            Key = key;
        }

        public EventCreationParams(string key, TableBookingConfig tableBookingConfig)
        {
            Key = key;
            TableBookingConfig = tableBookingConfig;
        }

        public EventCreationParams(string key, string socialDistancingRulesetKey)
        {
            Key = key;
            SocialDistancingRulesetKey = socialDistancingRulesetKey;
        }
    }
}