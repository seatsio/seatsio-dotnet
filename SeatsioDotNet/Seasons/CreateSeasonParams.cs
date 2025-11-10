using System;
using System.Collections.Generic;
using SeatsioDotNet.Charts;

namespace SeatsioDotNet.Events;

public class CreateSeasonParams
{
    public string Key { get; set; }
    public string Name { get; set; }
    public int? NumberOfEvents { get; set; }
    public IEnumerable<string> EventKeys { get; set; }
    public TableBookingConfig TableBookingConfig { get; set; }
    public Dictionary<string, object> ObjectCategories { get; set; }
    public Category[] Categories { get; set; }
    public List<Channel> Channels { get; set; }
    public ForSaleConfig ForSaleConfig { get; set; }
    public bool? ForSalePropagated { get; set; }

    public CreateSeasonParams WithKey(string key)
    {
        Key = key;
        return this;
    }

    public CreateSeasonParams WithTableBookingConfig(TableBookingConfig tableBookingConfig)
    {
        TableBookingConfig = tableBookingConfig;
        return this;
    }

    public CreateSeasonParams WithObjectCategories(Dictionary<string, object> objectCategories)
    {
        ObjectCategories = objectCategories;
        return this;
    }

    public CreateSeasonParams WithCategories(Category[] categories)
    {
        Categories = categories;
        return this;
    }

    public CreateSeasonParams WithName(string name)
    {
        Name = name;
        return this;
    }

    public CreateSeasonParams WithChannels(List<Channel> channels)
    {
        Channels = channels;
        return this;
    }

    public CreateSeasonParams WithForSaleConfig(ForSaleConfig forSaleConfig)
    {
        ForSaleConfig = forSaleConfig;
        return this;
    }   
    
    public CreateSeasonParams WithNumberOfEvents(int numberOfEvents)
    {
        NumberOfEvents = numberOfEvents;
        return this;
    }  
    
    public CreateSeasonParams WithEventKeys(IEnumerable<string> eventKeys)
    {
        EventKeys = eventKeys;
        return this;
    }   
    
    public CreateSeasonParams WithForSalePropagated(bool forSalePropagated)
    {
        ForSalePropagated = forSalePropagated;
        return this;
    }
}