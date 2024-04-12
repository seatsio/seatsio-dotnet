using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MarkEverythingAsForSaleTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        await Client.Events.MarkAsNotForSaleAsync(evnt.Key, new[] {"o1", "o2"}, null, new[] {"cat1", "cat2"});

        await Client.Events.MarkEverythingAsForSaleAsync(evnt.Key);
            
        Assert.Null((await Client.Events.RetrieveAsync(evnt.Key)).ForSaleConfig);
    }
}