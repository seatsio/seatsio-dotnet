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
        public Dictionary<string, object> ObjectCategories { get; set; }
        public Category[] Categories { get; set; }
        public bool? IsInThePast { get; set; }

        public UpdateEventParams WithKey(string key)
        {
            Key = key;
            return this;
        }    
        
        public UpdateEventParams WithChartKey(string chartKey)
        {
            ChartKey = chartKey;
            return this;
        }

        public UpdateEventParams WithTableBookingConfig(TableBookingConfig tableBookingConfig)
        {
            TableBookingConfig = tableBookingConfig;
            return this;
        }

        public UpdateEventParams WithObjectCategories(Dictionary<string, object> objectCategories)
        {
            ObjectCategories = objectCategories;
            return this;
        }

        public UpdateEventParams WithCategories(Category[] categories)
        {
            Categories = categories;
            return this;
        }

        public UpdateEventParams WithName(string name)
        {
            Name = name;
            return this;
        }

        public UpdateEventParams WithDate(DateOnly date)
        {
            Date = date;
            return this;
        }

        public UpdateEventParams WithIsInThePast(bool isInThePast)
        {
            IsInThePast = isInThePast;
            return this;
        }
    }
}