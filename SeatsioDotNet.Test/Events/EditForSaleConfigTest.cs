using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class EditForSaleConfigTest : SeatsioClientTest
{
    [Fact]
    public async Task MarkAsForSale()
    {
        var chartKey = CreateTestChart();
        ForSaleConfig forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new [] {"A-1", "A-2", "A-3"});
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        
        await Client.Events.EditForSaleConfigAsync(evnt.Key, new[] {new ObjectAndQuantity("A-1"), new ObjectAndQuantity("A-2")});

        var retrievedForSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig.ForSale);
        Assert.Equal(new[] {"A-3"}, retrievedForSaleConfig.Objects);
    } 
        
    
    [Fact]
    public async Task ReturnsResult()
    {
        var chartKey = CreateTestChart();
        ForSaleConfig forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new [] {"A-1", "A-2", "A-3"});
        var evnt = await Client.Events.CreateAsync(chartKey, new CreateEventParams().WithForSaleConfig(forSaleConfig));
        
        var result = await Client.Events.EditForSaleConfigAsync(evnt.Key, new[] {new ObjectAndQuantity("A-1"), new ObjectAndQuantity("A-2")});

        Assert.False(result.ForSaleConfig.ForSale);
        Assert.Equal(new[] {"A-3"}, result.ForSaleConfig.Objects);
        Assert.Equal(9, result.RateLimitInfo.RateLimitRemainingCalls);
    } 
    
    [Fact]
    public async Task MarkAsNotForSale()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        
        await Client.Events.EditForSaleConfigAsync(evnt.Key, null, new[] {new ObjectAndQuantity("A-1"), new ObjectAndQuantity("A-2")});

        var retrievedForSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig.ForSale);
        Assert.Equal(new[] {"A-1", "A-2"}, retrievedForSaleConfig.Objects);
    } 
    
    [Fact]
    public async Task MarkAreaPlacesAsNotForSale()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        
        await Client.Events.EditForSaleConfigAsync(evnt.Key, null, new[] {new ObjectAndQuantity("GA1", 5)});

        var retrievedForSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.False(retrievedForSaleConfig.ForSale);
        Assert.Equal(new() { { "GA1", 5 } }, retrievedForSaleConfig.AreaPlaces);
    }
}