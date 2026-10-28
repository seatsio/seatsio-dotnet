using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Charts;

public class ListEventsOfChartTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);
        var event3 = await Client.Events.CreateAsync(chartKey);
        await Client.Events.CreateAsync(CreateTestChart());

        var events = Client.Charts.Events(chartKey).AllAsync();

        Assert.Equal(new[] {event3.Key, event2.Key, event1.Key}, events.Select(e => e.Key));
    }

    [Fact]
    public async Task Paging()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);
        var event3 = await Client.Events.CreateAsync(chartKey);

        var firstPage = await Client.Charts.Events(chartKey).FirstPageAsync(2);
        var pageAfter = await Client.Charts.Events(chartKey).PageAfterAsync(firstPage.NextPageStartsAfter.Value, 2);

        Assert.Equal(new[] {event3.Key, event2.Key}, firstPage.Items.Select(e => e.Key));
        Assert.Equal(new[] {event1.Key}, pageAfter.Items.Select(e => e.Key));
    }
}
