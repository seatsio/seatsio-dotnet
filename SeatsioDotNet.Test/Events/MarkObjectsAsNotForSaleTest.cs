using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkObjectsAsNotForSaleTest : SeatsioClientTest
{
    [Fact]
    public async Task ObjectsCandCategories()
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
        
    [Fact]
    public async Task Objects()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.MarkAsNotForSaleAsync(evnt.Key, new[] {"o1", "o2"}, null, null);

        var forSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Equal(new[] {"o1", "o2"}, forSaleConfig.Objects);
        Assert.Empty(forSaleConfig.AreaPlaces);
        Assert.Empty(forSaleConfig.Categories);
    }  
        
    [Fact]
    public async Task Categories()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.MarkAsNotForSaleAsync(evnt.Key, null, null, new[] {"cat1", "cat2"});

        var forSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Empty(forSaleConfig.Objects);
        Assert.Empty(forSaleConfig.AreaPlaces);
        Assert.Equal(new[] {"cat1", "cat2"}, forSaleConfig.Categories);
    }
}