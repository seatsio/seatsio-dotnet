using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class RemoveEventFromPartialSeasonTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = Client.Seasons.Create(chartKey, eventKeys: new[] {"event1", "event2"});
        var partialSeason = Client.Seasons.CreatePartialSeason(topLevelSeason.Key, eventKeys: new[] {"event1", "event2"});

        var updatedPartialSeason = Client.Seasons.RemoveEventFromPartialSeason(topLevelSeason.Key, partialSeason.Key, "event2");

        Assert.Equal(new[] {"event1"}, updatedPartialSeason.Events.Select(e => e.Key));
    }
}