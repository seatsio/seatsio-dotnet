using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class EditForSaleConfigForEventsTest : SeatsioClientTest
{
    [Fact]
    public async Task MarkAsForSale()
    {
        var chartKey = CreateTestChart();
        ForSaleConfig forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new[] { "A-1", "A-2", "A-3" });
        var evnt1 = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        var evnt2 = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        var request = new Dictionary<string, ForSaleAndNotForSale>
        {
            { evnt1.Key, new ForSaleAndNotForSale(new[] { new ObjectAndQuantity("A-1") }, null) },
            { evnt2.Key, new ForSaleAndNotForSale(new[] { new ObjectAndQuantity("A-2") }, null) }
        };

        await Client.Events.EditForSaleConfigForEventsAsync(request);

        var retrievedForSaleConfig1 = (await Client.Events.RetrieveAsync(evnt1.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig1.ForSale);
        Assert.Equal(new[] { "A-2", "A-3" }, retrievedForSaleConfig1.Objects);

        var retrievedForSaleConfig2 = (await Client.Events.RetrieveAsync(evnt2.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig2.ForSale);
        Assert.Equal(new[] { "A-1", "A-3" }, retrievedForSaleConfig2.Objects);
    }

    [Fact]
    public async Task ReturnsResponse()
    {
        var chartKey = CreateTestChart();
        ForSaleConfig forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new[] { "A-1", "A-2", "A-3" });
        var evnt1 = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        var evnt2 = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        var request = new Dictionary<string, ForSaleAndNotForSale>
        {
            { evnt1.Key, new ForSaleAndNotForSale(new[] { new ObjectAndQuantity("A-1") }, null) },
            { evnt2.Key, new ForSaleAndNotForSale(new[] { new ObjectAndQuantity("A-2") }, null) }
        };

        var result = await Client.Events.EditForSaleConfigForEventsAsync(request);

        Assert.False(result[evnt1.Key].ForSaleConfig.ForSale);
        Assert.Equal(new[] { "A-2", "A-3" }, result[evnt1.Key].ForSaleConfig.Objects);
        Assert.Equal(9, result[evnt1.Key].RateLimitInfo.RateLimitRemainingCalls);

        Assert.False(result[evnt2.Key].ForSaleConfig.ForSale);
        Assert.Equal(new[] { "A-1", "A-3" }, result[evnt2.Key].ForSaleConfig.Objects);
        Assert.Equal(9, result[evnt2.Key].RateLimitInfo.RateLimitRemainingCalls);
    }

    [Fact]
    public async Task MarkAsNotForSale()
    {
        var chartKey = CreateTestChart();
        var evnt1 = await Client.Events.CreateAsync(chartKey);
        var evnt2 = await Client.Events.CreateAsync(chartKey);
        var request = new Dictionary<string, ForSaleAndNotForSale>
        {
            { evnt1.Key, new ForSaleAndNotForSale(null, new[] { new ObjectAndQuantity("A-1") }) },
            { evnt2.Key, new ForSaleAndNotForSale(null, new[] { new ObjectAndQuantity("A-2") }) }
        };

        await Client.Events.EditForSaleConfigForEventsAsync(request);

        var retrievedForSaleConfig1 = (await Client.Events.RetrieveAsync(evnt1.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig1.ForSale);
        Assert.Equal(new[] { "A-1" }, retrievedForSaleConfig1.Objects);

        var retrievedForSaleConfig2 = (await Client.Events.RetrieveAsync(evnt2.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig2.ForSale);
        Assert.Equal(new[] { "A-2" }, retrievedForSaleConfig2.Objects);
    }
}