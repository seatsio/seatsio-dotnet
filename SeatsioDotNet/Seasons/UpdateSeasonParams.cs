using System.Collections.Generic;
using SeatsioDotNet.Charts;

namespace SeatsioDotNet.Events;

public class UpdateSeasonParams
{
    public string Key { get; set; }
    public string Name { get; set; }
    public TableBookingConfig TableBookingConfig { get; set; }
    public Dictionary<string, object> ObjectCategories { get; set; }
    public Category[] Categories { get; set; }
    public bool? ForSalePropagated { get; set; }

    public UpdateSeasonParams WithKey(string key)
    {
        Key = key;
        return this;
    }

    public UpdateSeasonParams WithTableBookingConfig(TableBookingConfig tableBookingConfig)
    {
        TableBookingConfig = tableBookingConfig;
        return this;
    }

    public UpdateSeasonParams WithName(string name)
    {
        Name = name;
        return this;
    }
    
    public UpdateSeasonParams WithObjectCategories(Dictionary<string, object> objectCategories)
    {
        ObjectCategories = objectCategories;
        return this;
    }

    public UpdateSeasonParams WithCategories(Category[] categories)
    {
        Categories = categories;
        return this;
    }

    public UpdateSeasonParams WithForSalePropagated(bool forSalePropagated)
    {
        ForSalePropagated = forSalePropagated;
        return this;
    }
}