using System.Collections.Generic;
using System.Threading.Tasks;
using SeatsioDotNet.EventReports;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkObjectsAsNotForSaleTest : SeatsioClientTest
{
    [Fact]
    public async Task ObjectsAndCategories()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.MarkAsNotForSaleAsync(evnt.Key, new[] {"o1", "o2"}, new() {{"GA1", 3}}, new[] {"cat1", "cat2"});

        var forSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Equal(new[] {"o1", "o2"}, forSaleConfig.Objects);
        Assert.Equal(new() {{"GA1", 3}}, forSaleConfig.AreaPlaces);
        Assert.Equal(new[] {"cat1", "cat2"}, forSaleConfig.Categories);
    }
}