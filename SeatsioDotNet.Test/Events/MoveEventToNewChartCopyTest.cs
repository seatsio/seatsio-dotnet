using System.Threading.Tasks;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class MoveEventToNewChartCopyTest : SeatsioClientTest
{
    [Fact]
    public async Task TestEventIsMovedToNewChartCopy()
    {
        var chartKey = CreateTestChart();
        var evnt = await Client.Events.CreateAsync(chartKey);
        Event movedEvent = await Client.Events.MoveEventToNewChartCopy(evnt.Key);
        
        Assert.NotEqual(evnt.ChartKey, movedEvent.ChartKey);
        Assert.Equal(evnt.Key, movedEvent.Key);
    }
}