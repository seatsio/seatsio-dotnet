using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkObjectsAsNotForSaleTest : SeatsioClientTest
{
    [Fact]
    public void ObjectsCandCategories()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.MarkAsNotForSale(evnt.Key, new[] {"o1", "o2"}, new() {{"GA1", 3}}, new[] {"cat1", "cat2"});

        var forSaleConfig = Client.Events.Retrieve(evnt.Key).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Equal(new[] {"o1", "o2"}, forSaleConfig.Objects);
        Assert.Equal(new() {{"GA1", 3}}, forSaleConfig.AreaPlaces);
        Assert.Equal(new[] {"cat1", "cat2"}, forSaleConfig.Categories);
    }   
        
    [Fact]
    public void Objects()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.MarkAsNotForSale(evnt.Key, new[] {"o1", "o2"}, null, null);

        var forSaleConfig = Client.Events.Retrieve(evnt.Key).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Equal(new[] {"o1", "o2"}, forSaleConfig.Objects);
        Assert.Empty(forSaleConfig.AreaPlaces);
        Assert.Empty(forSaleConfig.Categories);
    }  
        
    [Fact]
    public void Categories()
    {
        var chartKey = CreateTestChart();
        var evnt = Client.Events.Create(chartKey);
        Client.Events.MarkAsNotForSale(evnt.Key, null, null, new[] {"cat1", "cat2"});

        var forSaleConfig = Client.Events.Retrieve(evnt.Key).ForSaleConfig;
        Assert.False(forSaleConfig.ForSale);
        Assert.Empty(forSaleConfig.Objects);
        Assert.Empty(forSaleConfig.AreaPlaces);
        Assert.Equal(new[] {"cat1", "cat2"}, forSaleConfig.Categories);
    }
}