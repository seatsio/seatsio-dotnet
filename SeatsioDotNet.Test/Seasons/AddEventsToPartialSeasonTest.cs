using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class AddEventsToPartialSeasonTest : SeatsioClientTest
{
    [Fact]
    public void Test()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = Client.Seasons.Create(chartKey, eventKeys: new[] {"event1", "event2"});
        var partialSeason = Client.Seasons.CreatePartialSeason(topLevelSeason.Key, partialSeasonKey: "aPartialSeason");

        var updatedPartialSeason = Client.Seasons.AddEventsToPartialSeason(topLevelSeason.Key, partialSeason.Key, new[] {"event1", "event2"});

        Assert.Equal(new[] {"event1", "event2"}, updatedPartialSeason.Events.Select(e => e.Key));
        Assert.Equal(new[] {updatedPartialSeason.Key}, updatedPartialSeason.Events[0].PartialSeasonKeysForEvent);
    }
}