using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MoveEventsToNewChartCopyTest : SeatsioClientTest
{
    [Fact]
    public async Task TestEventsAreMovedToOneNewChartCopy()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);

        var movedEvents = await Client.Events.MoveEventsToNewChartCopy(new[] { event2.Key, event1.Key });

        Assert.Equal(new[] { event2.Key, event1.Key }, movedEvents.Select(e => e.Key));
        Assert.NotEqual(chartKey, movedEvents[0].ChartKey);
        Assert.Equal(movedEvents[0].ChartKey, movedEvents[1].ChartKey);
    }
}
