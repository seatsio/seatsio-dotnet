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
        public Dictionary<string, object> ObjectCategories { get; set; }
        public Category[] Categories { get; set; }
        public List<Channel> Channels { get; set; }
        public ForSaleConfig ForSaleConfig { get; set; }

        public CreateEventParams WithKey(string key)
        {
            Key = key;
            return this;
        }

        public CreateEventParams WithTableBookingConfig(TableBookingConfig tableBookingConfig)
        {
            TableBookingConfig = tableBookingConfig;
            return this;
        }

        public CreateEventParams WithObjectCategories(Dictionary<string, object> objectCategories)
        {
            ObjectCategories = objectCategories;
            return this;
        }

        public CreateEventParams WithCategories(Category[] categories)
        {
            Categories = categories;
            return this;
        }

        public CreateEventParams WithName(string name)
        {
            Name = name;
            return this;
        }

        public CreateEventParams WithDate(DateOnly date)
        {
            Date = date;
            return this;
        }

        public CreateEventParams WithChannels(List<Channel> channels)
        {
            Channels = channels;
            return this;
        }

        public CreateEventParams WithForSaleConfig(ForSaleConfig forSaleConfig)
        {
            ForSaleConfig = forSaleConfig;
            return this;
        }
    }
}