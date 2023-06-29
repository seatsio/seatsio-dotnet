using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;

namespace SeatsioDotNet.Events
{
    public class CreateEventParams
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public DateOnly? Date { get; set; }
        public TableBookingConfig TableBookingConfig { get; set; }
        public string SocialDistancingRulesetKey { get; set; }
        public Dictionary<string, object> ObjectCategories { get; set; }
        public Category[] Categories { get; set; }

        public CreateEventParams withKey(string key)
        {
            Key = key;
            return this;
        }

        public CreateEventParams withTableBookingConfig(TableBookingConfig tableBookingConfig)
        {
            TableBookingConfig = tableBookingConfig;
            return this;
        }

        public CreateEventParams withSocialDistancingRulesetKey(string socialDistancingRulesetKey)
        {
            SocialDistancingRulesetKey = socialDistancingRulesetKey;
            return this;
        }

        public CreateEventParams withObjectCategories(Dictionary<string, object> objectCategories)
        {
            ObjectCategories = objectCategories;
            return this;
        }

        public CreateEventParams withCategories(Category[] categories)
        {
            Categories = categories;
            return this;
        }

        public CreateEventParams withName(string name)
        {
            Name = name;
            return this;
        }

        public CreateEventParams withDate(DateOnly date)
        {
            Date = date;
            return this;
        }
    }
}