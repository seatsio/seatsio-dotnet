using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Events;

public class ListEventsTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var event1 = await Client.Events.CreateAsync(chartKey);
        var event2 = await Client.Events.CreateAsync(chartKey);
        var event3 = await Client.Events.CreateAsync(chartKey);

        var events = await Client.Events.ListAllAsync().ToListAsync();

        Assert.Equal(new[] {event3.Key, event2.Key, event1.Key}, events.Select(e => e.Key));
    }

    [Fact]
    public async Task ListSeasons()
    {
        var chartKey = CreateTestChart();
        var season1 = await Client.Seasons.CreateAsync(chartKey);
        var season2 = await Client.Seasons.CreateAsync(chartKey).ConfigureAwait(false);
        var season3 = await Client.Seasons.CreateAsync(chartKey).ConfigureAwait(false);

        var seasons = Client.Events.ListAllAsync();

        Assert.Equal(new[] {true, true, true}, seasons.Select(s => s.IsSeason));
    }
}