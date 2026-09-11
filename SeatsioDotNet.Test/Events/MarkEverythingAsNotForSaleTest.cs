using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkEverythingAsNotForSaleTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);

        await Client.Events.MarkEverythingAsNotForSaleAsync(evnt.Key);

        var forSaleConfig = (await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig;
        Assert.NotNull(forSaleConfig);
        Assert.True(forSaleConfig.ForSale);
        Assert.Empty(forSaleConfig.Objects);
        Assert.Empty(forSaleConfig.AreaPlaces);
        Assert.Empty(forSaleConfig.Categories);
    }
}


