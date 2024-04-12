using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class CreateEventsInSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task EventsKeysCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);

        var events = await Client.Seasons.CreateEventsAsync(season.Key, eventKeys: new[] {"event1", "event2"});

        Assert.Equal(new[] {"event2", "event1"}, events.Select(e => e.Key));
        Assert.True(events[0].IsEventInSeason);
        Assert.Equal(season.Key, events[0].TopLevelSeasonKey);
    }

    [Fact]
    public async Task NumberOfEventsCanBePassedIn()
    {
        var chartKey = CreateTestChart();
        var season = await Client.Seasons.CreateAsync(chartKey);

        var events = await Client.Seasons.CreateEventsAsync(season.Key, numberOfEvents: 2);

        Assert.Equal(2, events.Length);
    }
}