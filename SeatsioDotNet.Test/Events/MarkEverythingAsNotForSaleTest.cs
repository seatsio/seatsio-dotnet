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
        Assert.False(forSaleConfig.ForSale);
        Assert.Null(forSaleConfig.Objects);
        Assert.Null(forSaleConfig.AreaPlaces);
        Assert.Null(forSaleConfig.Categories);
    }
}


