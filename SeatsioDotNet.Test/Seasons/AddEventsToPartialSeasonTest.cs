using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SeatsioDotNet.Test.Seasons;

public class AddEventsToPartialSeasonTest : SeatsioClientTest
{
    [Fact]
    public async Task Test()
    {
        var chartKey = CreateTestChart();
        var topLevelSeason = await Client.Seasons.CreateAsync(chartKey, eventKeys: new[] {"event1", "event2"});
        var partialSeason = await Client.Seasons.CreatePartialSeasonAsync(topLevelSeason.Key, partialSeasonKey: "aPartialSeason");

        var updatedPartialSeason = await Client.Seasons.AddEventsToPartialSeasonAsync(topLevelSeason.Key, partialSeason.Key, new[] {"event1", "event2"});

        Assert.Equal(new[] {"event1", "event2"}, updatedPartialSeason.Events.Select(e => e.Key));
        Assert.Equal(new[] {updatedPartialSeason.Key}, updatedPartialSeason.Events[0].PartialSeasonKeysForEvent);
    }
}