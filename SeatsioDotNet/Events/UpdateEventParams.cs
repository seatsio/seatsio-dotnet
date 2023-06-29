using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;

namespace SeatsioDotNet.Events
{
    public class UpdateEventParams
    {
        public string Key { get; set; }
        public string ChartKey { get; set; }
        public string Name { get; set; }
        public DateOnly? Date { get; set; }
        public TableBookingConfig TableBookingConfig { get; set; }
        public string SocialDistancingRulesetKey { get; set; }
        public Dictionary<string, object> ObjectCategories { get; set; }
        public Category[] Categories { get; set; }

        public UpdateEventParams withKey(string key)
        {
            Key = key;
            return this;
        }    
        
        public UpdateEventParams withChartKey(string chartKey)
        {
            ChartKey = chartKey;
            return this;
        }

        public UpdateEventParams withTableBookingConfig(TableBookingConfig tableBookingConfig)
        {
            TableBookingConfig = tableBookingConfig;
            return this;
        }

        public UpdateEventParams withSocialDistancingRulesetKey(string socialDistancingRulesetKey)
        {
            SocialDistancingRulesetKey = socialDistancingRulesetKey;
            return this;
        }

        public UpdateEventParams withObjectCategories(Dictionary<string, object> objectCategories)
        {
            ObjectCategories = objectCategories;
            return this;
        }

        public UpdateEventParams withCategories(Category[] categories)
        {
            Categories = categories;
            return this;
        }

        public UpdateEventParams withName(string name)
        {
            Name = name;
            return this;
        }

        public UpdateEventParams withDate(DateOnly date)
        {
            Date = date;
            return this;
        }
    }
}