using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkObjectsAsForSaleTest : SeatsioClientTest
{
    [Fact]
    public async Task ObjectsCandCategories()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.MarkAsForSaleAsync(evnt.Key, new[] {"o1", "o2"}, new() {{"GA1", 3}}, new[] {"cat1", "cat2"});

        var forSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.True(forSaleConfig.ForSale);
        Assert.Equal(new[] {"o1", "o2"}, forSaleConfig.Objects);
        Assert.Equal(new() {{"GA1", 3}}, forSaleConfig.AreaPlaces);
        Assert.Equal(new[] {"cat1", "cat2"}, forSaleConfig.Categories);
    }
}